# import config.
# You can change the default config with `make cnf="config_special.env" build`
cnf ?= config.env
include $(cnf)
export $(shell sed 's/=.*//' $(cnf))

# HELP
# This will output the help for each task
.PHONY: help

help: ## This help.
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "\033[36m%-30s\033[0m %s\n", $$1, $$2}' $(MAKEFILE_LIST)

.DEFAULT_GOAL := help

# Make TASKS
# DOCKER TASKS
run: ## Create and start containers
	docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

up: run ## Build all containers and run - [docker-compose] (Alias to run)

# Build the container
build: run ## Build all containers and run - [docker-compose]

# Build the container
rebuild: ## Rebuild all containers and run - [docker-compose]
	docker-compose -f docker-compose.yml -f docker-compose.override.yml up --force-recreate --build -d

debug: ## Create and start containers in debug
	docker-compose -f docker-compose.yml -f docker-compose.override.yml up --force-recreate --build

stop: ## Stop all running containers
	docker-compose stop

rm: ## Remove all running containers
	docker-compose rm

down: ## Stop all running containers 
	docker-compose down

clean: ## Remove all running containers and volumes
	docker-compose down --volumes --rmi all