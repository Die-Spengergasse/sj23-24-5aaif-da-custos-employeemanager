#!/bin/bash

cd EmployeeManager.Client
npm i && npm run build
cd ../EmployeeManager.Webapi
dotnet restore --no-cache
dotnet build --no-restore
dotnet watch run
