
# Custom SQL Server Docker Container README

## Overview

This README documents the custom SQL Server Docker container designed to run Microsoft SQL Server on Windows Server Core (`ltsc2019`). This custom solution addresses the need for a Windows-based container capable of running SQL Server, especially tailored for development environments where Linux containers may not suffice or specific Windows integrations are necessary.

## Why This Custom Image?

- **Compatibility**: Ensures SQL Server runs in environments that strictly require Windows containers.
- **Flexibility**: Provides a customizable SQL Server setup within a Docker container, allowing for specific configurations, pre-loaded databases, or additional tools as per development needs.
- **Development and Testing**: Facilitates an isolated SQL Server instance that matches production settings but is easily deployable for development and testing.

## Building the Docker Image

To build this custom SQL Server Docker image, navigate to the directory containing the Dockerfile, then run:

```bash
docker build -t custom-mssql-windows .
```

## Running the Container

Launch the container with the following command:

```bash
docker run -d -p 1433:1433 --name custom-mssql custom-mssql-windows
```

Replace `custom-mssql-windows` with your chosen image tag.

## Configuration Details

- **SQL Server Version**: The Dockerfile uses the official Microsoft base image for Windows Server Core (`ltsc2019`) and installs SQL Server through provided download links (`exe` and `box` files).
- **Environment Variables**: The image is configured using environment variables for the SA password (`SA_PASSWORD`) and acceptance of the SQL Server End User License Agreement (`ACCEPT_EULA`).

## Troubleshooting

### Verifying SQL Server Operation

1. **Accessing the Container**: Initiate an interactive PowerShell session inside your container:
    ```bash
    docker exec -it custom-mssql powershell
    ```

2. **Checking SQL Server Service**: Verify the SQL Server service is running:
    ```powershell
    Get-Service MSSQLSERVER
    ```
    The service should be in a `Running` state.

3. **Testing Database Connectivity**: Use `sqlcmd` to test connectivity to the SQL Server instance:
    ```powershell
    sqlcmd -S localhost -U SA -P "Your_password123" -Q "SELECT @@VERSION"
    ```
    Replace `"Your_password123"` with your SA password. Successful execution indicates the SQL Server is operational.

4. ** Try this as well **
   ```powershell
   Test-NetConnection -ComputerName host.docker.internal -Port 1433
   ```
5. While logged on to the container (running)
   ```powershell
   sqlcmd -S localhost -U SA -P "Your_password123" -Q "SELECT name FROM sys.databases"
   ```

### Common Issues

- **DNS Resolution**: If there are DNS resolution issues within the container, ensure it's configured to use reliable DNS servers (`8.8.8.8` and `8.8.4.4` are set as defaults).
- **Database Connection**: For connection issues, verify the container's port `1433` is mapped correctly and accessible.
- **Service Startup**: If SQL Server fails to start, review the container logs for errors and check if the `SA_PASSWORD` and `ACCEPT_EULA` environment variables are correctly set.

## Additional Notes

- This container is intended for development and testing. Evaluate security and performance for production use.
- Regularly check for updates to the base image and SQL Server installation files to maintain security and stability.
- Customization through the `start.ps1` script allows for advanced configurations, including attaching additional databases at startup.

---

This README provides a comprehensive guide to deploying and managing your custom SQL Server container. For further customization and advanced scenarios, refer to official Microsoft documentation and Docker's guide on managing containers.
