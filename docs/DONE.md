# Done

Finished work, newest first. A finished item's detail section is deleted from `docs/` when it lands here. To find it in git history, run `git log -S "## CODE " -- docs/`.

| Date | Code | Area | Change | Why / how |
|---|---|---|---|---|
| 2026-11-02 | — | Docs | Set up the TODO index, its detail docs, this log and `CHANGELOG.md`, and move `// TODO` comments into the index | Open work was spread across code comments, GitHub and unmerged branches; it's now indexed by code, and the changelog comes from each release's code and download. Verified with the audit script, a rebuild and a comment-only code diff. |
| 2026-11-02 | — | Docs | Add `CLAUDE.md` with build steps, architecture and JSON config rules | Sessions had nothing to start from, and `dotnet build` fails on this project. Verified by building with the documented command and loading every shipped config through the built classes. |
