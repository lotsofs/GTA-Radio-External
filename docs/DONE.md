# Done

Finished work, newest first. A finished item's detail section is deleted from `docs/` when it lands here. To find it in git history, run `git log -S "## CODE " -- docs/`.

| Date | Code | Area | Change | Why / how |
|---|---|---|---|---|
| 2026-11-02 | SPT (partial) | Music players | Add Spotify through an `AudioSession` hook that mutes an app in the Windows Volume Mixer | PR #5 couldn't merge into the active code, so its idea was ported with direct Core Audio calls, with no new dependency or device setting. Verified on real Spotify with a stand-in game: it muted and unmuted with the radio and other apps' sessions didn't change; not yet seen with a real game. |
| 2026-11-02 | STS | UI | Show live game and music player status in the window, including missing modules and errors from the poll | The labels were unwired placeholders and the timer swallowed poll errors; `Radio` now reports status from polls that can't overlap, and `FormRE` shows it. Verified by driving the built exe with stand-in processes, including a stand-in player muted and restored; not yet seen with a real game. |
| 2026-11-02 | — | Docs | Set up the TODO index, its detail docs, this log and `CHANGELOG.md`, and move `// TODO` comments into the index | Open work was spread across code comments, GitHub and unmerged branches; it's now indexed by code, and the changelog comes from each release's code and download. Verified with the audit script, a rebuild and a comment-only code diff. |
| 2026-11-02 | — | Docs | Add `CLAUDE.md` with build steps, architecture and JSON config rules | Sessions had nothing to start from, and `dotnet build` fails on this project. Verified by building with the documented command and loading every shipped config through the built classes. |
