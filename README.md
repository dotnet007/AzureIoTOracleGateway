# AzureIoT OracleGateway
This software was made to help people or companies to read data from an Oracle database and send that to Azure IoT Hub.

Please take in mind the following recommendations:

1. Download the solution AzureIoT and open it in VS 20017
2. Compile and the in the output directory (debug) open the followin files:
2.1. AzureIoT.OracleGateway.exe.config and update the required AppSettings
2.2. Open the file query.sql and update with your oracle query.
2.3. Open the file UltimaFecha.json and update the date.
2.4. Intsall the Oracle driver in your machine and configure the TSNAMES file.

## Overview description of the Oracle Gateway

This Gateway was developed in the following way:
The program is a batch or comand line application, that will query an oracle database every X milliseconds (according to the configuration file).
The query will retun a group of rows, and the Gateway are going to send to Azure IoT Hub each message per row.
One of the column of the query must to have the name of the device, so you must to create and update the configuration file with all the connection strings of the devices created in the IoT Hub.
One of the column of the query must to have the date, that will be the stating point or reference from when you want to get the rows.

## FAQ or help

If you need any help please create any issue or contact me directly by this way (github).

Thanks and enjoy!
