[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/fund.html)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/dynamic-dns-updater-client)](https://github.com/hmlendea/dynamic-dns-updater-client/releases/latest)
[![Build Status](https://github.com/hmlendea/dynamic-dns-updater-client/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/dynamic-dns-updater-client/actions/workflows/dotnet.yml)
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://gnu.org/licenses/gpl-3.0)

# About
CLI application that updates a DNS record through the Dynamic DNS Updater API.

# What It Does

This tool:

- Detects the current public IP address of the machine
- Sends an authenticated `PUT` request to the Dynamic DNS Updater API
- Updates the DNS record for a given domain using a selected DNS provider

It is useful for keeping DNS records in sync when your external IP changes.

## ✨ Capabilities

- Detects the current public IP address of the machine
- Sends an authenticated `PUT` request to the Dynamic DNS Updater API
- Updates the DNS record for a given domain using a selected DNS provider
- Keeps DNS records in sync when your external IP changes

## 🚀 Usage

### Requirements

- .NET SDK 10.0+
- Access to a running Dynamic DNS Updater API instance
- A valid API key for that instance

### Configuration

The app reads configuration from `appsettings.json`.

```json
{
	"apiSettings": {
		"baseUrl": "https://your-api.example.com",
		"apiKey": "your-api-key"
	}
}
```

#### Settings

- `apiSettings.baseUrl`: Base URL of the Dynamic DNS Updater API
- `apiSettings.apiKey`: Bearer token used to authenticate requests

### CLI Options

Run with `dotnet run`:

```bash
dotnet run -- -d your-domain.example.com -p your-provider
```

Or run the built binary:

```bash
./DynamicDnsUpdater.Client -d your-domain.example.com -p your-provider
```

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

### Example

```bash
dotnet run -- --domain-name example.com --provider-name cloudflare
```

## 🛠️ Build

```bash
dotnet build
```

To publish a release build:

```bash
dotnet publish -c Release
```

## 📝 Logging

The application logs operation start, success, and failure states through `NuciLog`, including:

- Domain name
- Provider name
- API base URL
- Detected public IP
- API response code/message (on failure)

## 📋 Notes

- The client sends requests to `DnsRecords/{domainName}` on the configured API.
- The `provider` value is passed through to the API as-is; accepted values depend on server-side configuration.

## 🛡️ Privacy and Data

Dynamic DNS Updater Client is a self-hosted command-line application that does not collect, store, or transmit personal data beyond what is necessary to update DNS records via the configured API. The application detects the public IP address and sends it along with the domain name, provider name, and API key to the Dynamic DNS Updater API endpoint configured by the operator.

For the detailed description of how the application handles privacy and personal data, see [PRIVACY.md](./PRIVACY.md).

## 📑 Table of Contents

- [Capabilities](#-capabilities)
- [Usage](#-usage)
  - [CLI Options](#-cli-options)
  - [Example](#-example)
- [Build](#-build)
- [Logging](#-logging)
- [Notes](#-notes)
- [Contributing](#-contributing)
- [Project Engagement](#-project-engagement)
- [License](#-license)

## 🤝 Contributing

You are welcome to submit any suggestion, feedback, or modification to this project.

When doing so, please:
- Maintain cross-platform compatibility
- Submit focused pull requests that conform to the existing code style
- Maintain your branch synchronised with `master`
- Revise the documentation when functionality changes
- Properly test all modifications, including edge cases and error conditions
- Include tests for additional or modified functionality
- Raise a new [issue](https://github.com/hmlendea/dynamic-dns-updater-client/issues) for problems or suggestions

See the [contributing guidelines](./CONTRIBUTING.md) for details on how to report issues, suggest enhancements, and submit changes.

## 📜 Code of Conduct

See the [Code of Conduct](CODE_OF_CONDUCT.md) for the standards of conduct expected in all project spaces.

## 🔒 Security

Dynamic DNS Updater Client follows a coordinated disclosure policy for security vulnerabilities. Only the latest version from GitHub Releases or the master branch receives security maintenance.

For information on reporting security vulnerabilities, see [SECURITY.md](./SECURITY.md).

## 💝 Project Engagement

Discovered a problem or have a suggestion? [Open an issue](https://github.com/hmlendea/dynamic-dns-updater-client/issues)!

If you find this project useful, consider [funding it](https://hmlendea.go.ro/funding) or starring ⭐️ it on GitHub!

[![Donate](https://raw.githubusercontent.com/hmlendea/readme-assets/master/donate_generic.png)](https://hmlendea.go.ro/funding)

## 📄 License

This project is licensed under the GPLv3 License. See `LICENSE` for details.
