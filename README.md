[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html) [![Latest Release](https://img.shields.io/github/v/release/hmlendea/dynamic-dns-updater-client)](https://github.com/hmlendea/dynamic-dns-updater-client/releases/latest) [![Build Status](https://github.com/hmlendea/dynamic-dns-updater-client/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/dynamic-dns-updater-client/actions/workflows/dotnet.yml)

# About
CLI application that updates a DNS record through the Dynamic DNS Updater API.

# What It Does

This tool:

- Detects the current public IP address of the machine
- Sends an authenticated `PUT` request to the Dynamic DNS Updater API
- Updates the DNS record for a given domain using a selected DNS provider

It is useful for keeping DNS records in sync when your external IP changes.

# Requirements

- .NET SDK 10.0+
- Access to a running Dynamic DNS Updater API instance
- A valid API key for that instance

# Configuration

The app reads configuration from `appsettings.json`.

```json
{
	"apiSettings": {
		"baseUrl": "https://your-api.example.com",
		"apiKey": "your-api-key"
	}
}
```

## Settings

- `apiSettings.baseUrl`: Base URL of the Dynamic DNS Updater API
- `apiSettings.apiKey`: Bearer token used to authenticate requests

# Usage

Run with `dotnet run`:

```bash
dotnet run -- -d your-domain.example.com -p your-provider
```

Or run the built binary:

```bash
./DynamicDnsUpdater.Client -d your-domain.example.com -p your-provider
```

## CLI Options

Domain name options:

- `-d`
- `--dn`
- `--domain`
- `--domain-name`

Provider name options:

- `-p`
- `--pn`
- `--provider`
- `--provider-name`

# Example

```bash
dotnet run -- --domain-name example.com --provider-name cloudflare
```

# Build

```bash
dotnet build
```

To publish a release build:

```bash
dotnet publish -c Release
```

# Logging

The application logs operation start, success, and failure states through `NuciLog`, including:

- Domain name
- Provider name
- API base URL
- Detected public IP
- API response code/message (on failure)

# Notes

- The client sends requests to `DnsRecords/{domainName}` on the configured API.
- The `provider` value is passed through to the API as-is; accepted values depend on server-side configuration.

# License

This project is licensed under the GPLv3 License. See `LICENSE` for details.
