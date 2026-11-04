# Contributing

Open a pull request against `main`. Keep each change reviewable, explain the
concrete problem, and include validation results and remaining limits. Use a
Conventional Commit PR title so squash merges preserve release intent:

```
feat(protocol): decode an additional verified response
fix(resume): reject invalid cached fan bounds
docs: record an unsupported hardware route
```

`feat` adds a feature, `fix` repairs behavior, and `!` or a `BREAKING CHANGE:` footer
declares an incompatible change. `docs`, `ci`, `test`, `build`, `refactor`, `perf`,
`style`, `chore`, and `revert` describe other changes. Release-impacting changes
should use `feat` or `fix`; do not hide behavior changes in a documentation commit.

Run the README checks before submitting. Keep dependency lockfiles and update
them with the associated manifests. Actions must use immutable commit SHAs;
Dependabot proposes updates for review. Do not bulk-format historical C# output.

Publish only reduced, relevant evidence. Do not commit original packages,
executables, private paths, serial numbers, credentials, raw memory, runtime
captures, or deployment backups. A small explicitly allowlisted protocol fixture
is the exception. Never infer a successful physical effect from firmware readback
alone. Label observations, source deductions, and untested behavior separately.

CI is deliberately offline with respect to hardware. Changes that write firmware,
fan settings, power limits, or persistent LED state need separate live validation
and recovery evidence. GitHub-hosted runners must never execute those tests.

Keep `VERSION`, the release manifest, and the changelog consistent. Normally the
release bot updates them together; review its PR rather than manually tagging.
