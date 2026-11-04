"""Dispatch read-only CI for release PRs without a long-lived token."""

import json
import os
import re
import urllib.request


def release_branches(prs):
    """Refuse unexpected refs before making a privileged API request."""
    branches = set()
    for pr in prs:
        branch = pr["headBranchName"]
        if pr["baseBranchName"] != "main" or not re.fullmatch(
            r"release-please--branches--main(?:--[A-Za-z0-9_.-]+)?", branch
        ):
            raise ValueError("Unexpected release PR branch")
        branches.add(branch)
    return sorted(branches)


def main():
    branches = release_branches(json.loads(os.environ["RELEASE_PRS"]))
    api = os.environ["GITHUB_API_URL"].rstrip("/")
    repository = os.environ["GITHUB_REPOSITORY"]
    for branch in branches:
        request = urllib.request.Request(
            f"{api}/repos/{repository}/actions/workflows/ci.yml/dispatches",
            data=json.dumps({"ref": branch}).encode(),
            headers={
                "Authorization": f"Bearer {os.environ['GH_TOKEN']}",
                "Accept": "application/vnd.github+json",
                "X-GitHub-Api-Version": "2022-11-28",
            },
            method="POST",
        )
        with urllib.request.urlopen(request, timeout=30) as response:
            if response.status != 204:
                raise RuntimeError("CI dispatch did not return HTTP 204")
        print(f"Dispatched CI for {branch}")


if __name__ == "__main__":
    main()
