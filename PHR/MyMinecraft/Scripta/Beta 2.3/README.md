# Beta 2.3
### Quick log:
- player inventory
- debug screen (fps, player, ...)
- edge crouching
- auto climbing on small stuff
- improvemnts
  - universal player input handler
- bug fixy
  - hit/place position
  - jump
  - block placing on chunk edge
  - check if player fits when exiting crouch/crawl/...
## Kinda full log:
- debug screen - ukazuje veci jako (fps, player location {svet, chunk}, cpu, gpu, memory usage, ...
- edge crouching - player nepada z blocku kdyz crouchuje (obcas to nefunguje - nvm proc)
- check if player fits when exiting crouch/crawl/... - deted pokud player sedel a byl pod blockem kdy si stoupl byl uvnitr blocku
