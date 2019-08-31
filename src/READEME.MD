# Setup

Setup required services via docker compose:
`sh infra/infra.up.sh`

# InfluxDb

## Configuration
- URL: http://influxdb:8086

# Run

- Run Api `dotnet run --project ./src/Api/Api.csproj`
- Run Upstream - Sample A `dotnet run --project ./src/ApiToUse/ApiToUse.csproj`
- Run Upstream - Sample B `dotnet run --project ./src/ApiToTest/ApiToTest.csproj`
- Api address http://localhost:8080/api/values
- Kibana: http://localhost:5601/app/kibana
- Grafana: http://localhost:3000/
- Metrics:
  - Values' Upstream counter: http://localhost:3000/explore?orgId=1&left=%5B%22now-5m%22,%22now%22,%22InfluxDB%22,%7B%22datasource%22:%22InfluxDB%22,%22policy%22:%22default%22,%22resultFormat%22:%22time_series%22,%22orderByTime%22:%22ASC%22,%22tags%22:%5B%5D,%22groupBy%22:%5B%7B%22type%22:%22time%22,%22params%22:%5B%22$__interval%22%5D%7D,%7B%22type%22:%22fill%22,%22params%22:%5B%22null%22%5D%7D%5D,%22select%22:%5B%5B%7B%22type%22:%22field%22,%22params%22:%5B%22value%22%5D%7D,%7B%22type%22:%22mean%22,%22params%22:%5B%5D%7D%5D%5D,%22measurement%22:%22application__upstream_api_call%22%7D,%7B%22mode%22:%22Metrics%22%7D,%7B%22ui%22:%5Btrue,true,false,%22none%22%5D%7D%5D
  - Api experiment time graph: http://localhost:3000/explore?orgId=1&left=%5B%22now-5m%22,%22now%22,%22InfluxDB%22,%7B%22datasource%22:%22InfluxDB%22,%22policy%22:%22default%22,%22resultFormat%22:%22time_series%22,%22orderByTime%22:%22ASC%22,%22tags%22:%5B%5D,%22groupBy%22:%5B%7B%22type%22:%22time%22,%22params%22:%5B%22$__interval%22%5D%7D,%7B%22type%22:%22fill%22,%22params%22:%5B%22null%22%5D%7D%5D,%22select%22:%5B%5B%7B%22type%22:%22field%22,%22params%22:%5B%22samples%22%5D%7D,%7B%22type%22:%22mean%22,%22params%22:%5B%5D%7D%5D%5D,%22measurement%22:%22application__getvalues_touse_timer%22%7D,%7B%22datasource%22:%22InfluxDB%22,%22policy%22:%22default%22,%22resultFormat%22:%22time_series%22,%22orderByTime%22:%22ASC%22,%22tags%22:%5B%5D,%22groupBy%22:%5B%7B%22type%22:%22time%22,%22params%22:%5B%22$__interval%22%5D%7D,%7B%22type%22:%22fill%22,%22params%22:%5B%22null%22%5D%7D%5D,%22select%22:%5B%5B%7B%22type%22:%22field%22,%22params%22:%5B%22samples%22%5D%7D,%7B%22type%22:%22mean%22,%22params%22:%5B%5D%7D%5D%5D,%22measurement%22:%22application__getvalues_totry_timer%22%7D,%7B%22mode%22:%22Metrics%22%7D,%7B%22ui%22:%5Btrue,true,false,%22none%22%5D%7D%5D
