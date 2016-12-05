pwd
ls -la
docker run --rm -v /vsts/agent/_work/1/s/Shop.Client:/app -w="/app" node:6.9.1 /bin/bash -c "ls -la"
docker run --rm -v $(pwd):/app -w="/app" node:6.9.1 /bin/bash -c "ls -la Shop.Client"

