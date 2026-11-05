import sys
from pathlib import Path
import unittest

sys.path.insert(0, str(Path(__file__).resolve().parents[1]))
from check_repository import COMMIT, SEMVER, publication_error, workflow_errors
from dispatch_release_ci import release_branches


class PublicationTests(unittest.TestCase):
    def test_local_captures_and_secrets_are_rejected(self):
        for path in ["runtime/capture.json", "decomplied/output.cs", "Compiled Original Binaries/a.msix",
                     "backend/target/debug/service.exe", ".env", ".env.production", "private.key",
                     "decompiled_service/new-driver.sys", "desktop/other.ico", "desktop/App/obj/generated.cs"]:
            with self.subTest(path=path):
                self.assertIsNotNone(publication_error(path))

    def test_sources_lockfiles_and_single_fixture_are_allowed(self):
        for path in ["backend/Cargo.lock", "backend/crates/a/src/lib.rs", ".env.example", "desktop/NucControl.App/Assets/NucControl.ico",
                     "backend/crates/nuc11ph-protocol/tests/fixtures/test036-fan-profile.bin"]:
            self.assertIsNone(publication_error(path))
        self.assertIsNotNone(publication_error("backend/another.bin"))

    def test_version_rejects_ambiguous_or_decorated_versions(self):
        self.assertIsNotNone(SEMVER.fullmatch("0.1.0"))
        for value in ["v0.1.0", "01.1.0", "1.2", "1.0.0-beta", "1.2.3\n4.5.6"]:
            self.assertIsNone(SEMVER.fullmatch(value))

    def test_commit_title_accepts_scopes_and_breaking_changes(self):
        for title in ["fix(resume): reject invalid state", "feat!: change protocol", "chore(main): release 0.2.0"]:
            self.assertIsNotNone(COMMIT.fullmatch(title))
        for title in ["Update stuff", "fix: ", "fix: good\nmalicious", "feat: missing\n"]:
            self.assertIsNone(COMMIT.fullmatch(title))

    def test_workflow_rejects_mutable_actions_and_privileged_events(self):
        workflow = {"on": {"pull_request_target": {}}, "permissions": {"contents": "write"},
                    "jobs": {"test": {"runs-on": "self-hosted", "steps": [{"uses": "actions/checkout@main"}]}}}
        self.assertEqual(len(workflow_errors(workflow)), 5)

    def test_release_dispatch_rejects_unexpected_refs(self):
        valid = {"headBranchName": "release-please--branches--main", "baseBranchName": "main"}
        self.assertEqual(release_branches([valid, valid]), [valid["headBranchName"]])
        for pr in [{**valid, "headBranchName": "main"}, {**valid, "baseBranchName": "other"},
                   {**valid, "headBranchName": "release-please--branches--main/../../bad"}]:
            with self.assertRaises(ValueError):
                release_branches([pr])


if __name__ == "__main__":
    unittest.main()
