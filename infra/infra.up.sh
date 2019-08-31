#!/bin/sh

DIR="${0%/*}"

cd $DIR
docker-compose up -d
