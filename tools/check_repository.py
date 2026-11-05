"""Validate maintained metadata and prevent accidental publication of local data."""

import argparse
import hashlib
import json
import os
from pathlib import Path, PurePosixPath
import re
import subprocess

import yaml

ROOT = Path(__file__).resolve().parents[1]
SEMVER = re.compile(r"(?:0|[1-9][0-9]*)\.(?:0|[1-9][0-9]*)\.(?:0|[1-9][0-9]*)")
COMMIT = re.compile(
    r"(?:feat|fix|docs|ci|chore|refactor|perf|test|build|style|revert)"
    r"(?:\([a-zA-Z0-9_./-]+\))?!?: .+"
)
LOCAL_ROOTS = {"runtime", "decomplied", "Compiled Original Binaries", ".repo-tools"}
LEGACY_ROOTS = {"decompiled_app", "decompiled_service"}
BINARY_SUFFIXES = {
    ".ico",
    ".exe", ".dll", ".sys", ".msi", ".msix", ".appx", ".pdb", ".dmp",
    ".hdmp", ".wer", ".etl", ".zip", ".7z", ".bin", ".pem", ".key", ".pfx", ".p12",
}
FIXTURE = "backend/crates/nuc11ph-protocol/tests/fixtures/test036-fan-profile.bin"
FIXTURE_SHA = "ae29a177c99a976182dc14579bdf7c028bac1df0a14c503bbc35448d1cc30f68"
OWNED_ICON = "desktop/NucControl.App/Assets/NucControl.ico"
OWNED_ICON_SHA = "eec4b0337dc5fdbcd871a2df0bd8b28dfa58083d847d0ce60edd6dc676e0bbd9"
LEGACY_BINARIES = {
    "decompiled_app/NucSoftwareStudio/app.ico",
    "decompiled_app/NucSoftwareStudio/nuc_icon.ico",
    "decompiled_service/OpenHardwareMonitorLib/OpenHardwareMonitor.Hardware.WinRing0.sys",
    "decompiled_service/OpenHardwareMonitorLib/OpenHardwareMonitor.Hardware.WinRing0x64.sys",
}


def publication_error(name):
    path = PurePosixPath(name)
    if path.parts[0] in LOCAL_ROOTS:
        return "local investigation directory is not publishable"
    if path.suffix.lower() in BINARY_SUFFIXES and name not in LEGACY_BINARIES | {FIXTURE, OWNED_ICON}:
        return "binary, private key, or runtime artifact is not publishable"
    if path.name == ".env" or path.name.startswith(".env.") and path.name != ".env.example":
        return "environment secrets are not publishable"
    if any(part in {"target", "node_modules", "__pycache__", ".venv", "bin", "obj", "artifacts", "TestResults"} for part in path.parts):
        return "generated dependency or build directory is not publishable"
    return None


def workflow_errors(workflow):
    errors = []
    triggers = workflow.get("on", {})
    if not isinstance(triggers, dict) or "pull_request_target" in triggers:
        errors.append("workflow must use explicit safe event mappings")
    if workflow.get("permissions", {}).get("contents") != "read":
        errors.append("default workflow token must be read-only")
    for job in workflow.get("jobs", {}).values():
        if "timeout-minutes" not in job:
            errors.append("job is missing a timeout")
        if job.get("runs-on") not in {"ubuntu-24.04", "windows-2022"}:
            errors.append("only explicitly selected GitHub-hosted runners are allowed")
        for step in job.get("steps", []):
            action = step.get("uses", "")
            if action and not re.fullmatch(r"[\w.-]+/[\w./-]+@[0-9a-f]{40}", action):
                errors.append(f"action must be pinned to a commit SHA: {action}")
    return errors


def check(root):
    errors = []
    version = (root / "VERSION").read_text().strip()
    manifest = json.loads((root / ".release-please-manifest.json").read_text())
    if not SEMVER.fullmatch(version) or manifest.get(".") != version:
        errors.append("VERSION must be stable SemVer and match the release manifest")
    changelog = (root / "CHANGELOG.md").read_text(encoding="utf-8")
    if not re.search(rf"^## \[?{re.escape(version)}(?:\]|\s|$)", changelog, re.MULTILINE):
        errors.append("CHANGELOG must contain the current version heading")
    config = json.loads((root / "release-please-config.json").read_text())
    package = config["packages"]["."]
    if package.get("version-file") != "VERSION" or package.get("release-type") != "simple":
        errors.append("release configuration must update the research bundle VERSION")
    for file in (root / ".github/workflows").glob("*.yml"):
        workflow = yaml.load(file.read_text(encoding="utf-8"), Loader=yaml.BaseLoader)
        errors.extend(f"{file.name}: {error}" for error in workflow_errors(workflow))
    files = subprocess.check_output(
        ["git", "ls-files", "-z", "--cached", "--others", "--exclude-standard"], cwd=root
    ).decode("utf-8").split("\0")
    for name in filter(None, files):
        error = publication_error(name)
        if error:
            errors.append(f"{name}: {error}")
            continue
        if PurePosixPath(name).parts[0] in LEGACY_ROOTS:
            continue
        path = root / name
        if not path.is_file():
            errors.append(f"{name}: tracked file is missing")
            continue
        if name == OWNED_ICON:
            if hashlib.sha256(path.read_bytes()).hexdigest() != OWNED_ICON_SHA:
                errors.append("owned icon changed; review provenance and allowlist")
            continue
        if name == FIXTURE:
            if len(path.read_bytes()) != 94 or hashlib.sha256(path.read_bytes()).hexdigest() != FIXTURE_SHA:
                errors.append("captured fan-profile fixture has changed; review provenance and allowlist")
            continue
        if path.stat().st_size > 4 * 1024 * 1024:
            errors.append(f"{name}: unexpectedly large publication input")
        text = path.read_text(encoding="utf-8")
        if re.search(r"[A-Za-z]:[\\/]Users[\\/]", text):
            errors.append(f"{name}: contains a private absolute user path")
        if re.search(r"gh[pousr]_[A-Za-z0-9]{30,}|github_pat_[A-Za-z0-9_]{30,}|-----BEGIN "
                     r".*PRIVATE KEY-----", text):
            errors.append(f"{name}: possible credential material")
    return errors


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--pr-title", action="store_true")
    args = parser.parse_args()
    if args.pr_title:
        event = json.loads(Path(os.environ["GITHUB_EVENT_PATH"]).read_text(encoding="utf-8"))
        title = event["pull_request"]["title"]
        errors = [] if COMMIT.fullmatch(title) else ["PR title must be a Conventional Commit"]
    else:
        errors = check(ROOT)
    for error in errors:
        print(error)
    if errors:
        raise SystemExit(1)
    print("Repository checks passed.")


if __name__ == "__main__":
    main()
