pwd
ls -la
docker run --rm -v $(pwd)/Shop.Client:/app node:6.9.1 -w="/app" npm install
