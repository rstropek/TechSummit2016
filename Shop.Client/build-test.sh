pwd
ls -la
docker run --rm -v $(pwd)/Shop.Client:/app -w="/app" node:6.9.1 /bin/bash -c "ls -la"
