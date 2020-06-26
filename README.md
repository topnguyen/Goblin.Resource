# Docker Build and Run

docker build --tag goblin-resource:1.0 .

docker run --network bridge --publish 8001:80 --env-file DockerEnv --detach --name goblin-resource goblin-resource:1.0

---

# Docker Remove

docker rm --force goblin-resource

---

# Network

docker network ls

docker network create -d bridge goblin

docker network inspect goblin

docker network rm goblin