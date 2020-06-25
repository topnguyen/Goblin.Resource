# Docker Build and Run

docker build --tag goblin-service-resource:1.0 .

docker run --network bridge --publish 8001:80 --env-file DockerEnv --detach --name goblin-service-resource goblin-service-resource:1.0

---

# Docker Remove

docker rm --force goblin-service-resource

---

# Network

docker network inspect bridge