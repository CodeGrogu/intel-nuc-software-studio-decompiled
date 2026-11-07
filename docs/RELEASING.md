# Versioning and releases

The research bundle uses SemVer in `VERSION` and `.release-please-manifest.json`.
It starts at 0.1.0. This version describes the maintained source and research
bundle, not Intel's installer or the inherited individual Cargo package versions.
During 0.x development, features bump the minor version, fixes bump the patch,
and breaking changes bump the minor version. At 1.x, breaking changes bump major.

1. Merge reviewed PRs with Conventional Commit squash titles. CI must pass.
2. Release Please maintains a release PR containing `VERSION`, the manifest, and
   `CHANGELOG.md`. Review the changes and evidence before merging that PR.
3. After merge, the bot creates a `vX.Y.Z` tag and GitHub release with changelog
   notes. No installer, executable, driver, or machine capture is uploaded.

The workflow uses GitHub's short-lived `GITHUB_TOKEN`. Because bot-created PR
events do not normally trigger other workflows, it explicitly dispatches the CI
workflow on the release PR's branch after each bot update. This requires the CI
workflow to have landed on `main`. No personal access token is required.

Repository Settings > Actions > General must allow GitHub Actions to create and
approve pull requests. The bot creates PRs but does not approve or merge them.
Keep the repository's default workflow token read-only; the release job alone
requests contents, issues, pull requests, and workflow-dispatch write access.

Recommended owner settings: squash merging with the PR title, delete merged
branches, protect `main` against force pushes/deletion, and require the CI checks
after their first successful run. Do not require an independent approval that a
sole maintainer cannot provide. These owner policies are separate from workflow
files and may require a repository plan that supports rulesets.

Dependabot checks Actions, Cargo, NuGet, and Python dependencies weekly. Review changes,
especially build scripts, before merging. Rust toolchain upgrades are explicit PRs.
Use the Actions UI to rerun CI or dispatch the release workflow on `main` when
recovering from a transient failure; never bypass a failed publication check.

References: [SemVer](https://semver.org/),
[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/),
[Release Please](https://github.com/googleapis/release-please-action), and
[GitHub token permissions](https://docs.github.com/en/actions/security-for-github-actions/security-guides/automatic-token-authentication).
