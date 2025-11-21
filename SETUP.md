# NeonArcade Setup Guide

## Initial Setup

### 1. Configure Development Settings

Copy the template file and update with your credentials:

```bash
# Copy the template
cp NeonArcade.Server/appsettings.Development.json.template NeonArcade.Server/appsettings.Development.json
```

Then edit `appsettings.Development.json` and update:
- `AdminUser:Email` - Your admin email
- `AdminUser:Password` - Your secure admin password (must meet password requirements)

**Important:** The `appsettings.Development.json` file is git-ignored and will not be committed to the repository.

### 2. Password Requirements

The admin password must meet the following requirements:
- At least 6 characters
- Contains at least one uppercase letter
- Contains at least one lowercase letter
- Contains at least one digit
- Non-alphanumeric characters are optional

### 3. Database Setup

Run the following commands to set up the database:

```bash
cd NeonArcade.Server
dotnet ef database update
```

### 4. Run the Application

```bash
dotnet run
```

The admin user will be automatically created on first run with the credentials from your configuration file.

## Security Notes

- **Never commit** `appsettings.Development.json` or `appsettings.Production.json` to version control
- Use the `.template` files as reference
- For production, use environment variables or Azure Key Vault for sensitive data
