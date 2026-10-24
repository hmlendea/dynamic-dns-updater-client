[![Donate](https://img.shields.io/badge/-%E2%99%A5%20Donate-%23ff69b4)](https://hmlendea.go.ro/funding)
[![Latest Release](https://img.shields.io/github/v/release/hmlendea/dynamic-dns-updater-client)](https://github.com/hmlendea/dynamic-dns-updater-client/releases/latest)
[![Build Status](https://github.com/hmlendea/dynamic-dns-updater-client/actions/workflows/dotnet.yml/badge.svg)](https://github.com/hmlendea/dynamic-dns-updater-client/actions/workflows/dotnet.yml)
[![License](https://img.shields.io/github/license/hmlendea/dynamic-dns-updater-client)](https://github.com/hmlendea/dynamic-dns-updater-client/blob/master/LICENSE)

# Dynamic DNS Updater Client

CLI application that updates a DNS record through the Dynamic DNS Updater API.

## 📑 Table of Contents

- [Capabilities](#-capabilities)
- [Usage](#-usage)
  - [CLI Options](#-cli-options)
  - [Example](#-example)
- [Installation](#-installation)
- [Configuration](#-configuration)
  - [Configuration Files](#-configuration-files)
  - [Settings](#-settings)
  - [Command-Line Options](#-command-line-options)
- [Development](#-development)
  - [Requirements](#-requirements)
  - [Setup](#-setup)
  - [Build](#-build)
  - [Run](#-run)
  - [Test](#-test)
  - [Release](#-release)
- [GitHub Actions](#-github-actions)
- [Project Structure](#-project-structure)
  - [Projects and Packages](#-projects-and-packages)
- [Contributing](#-contributing)
- [Code of Conduct](#-code-of-conduct)
- [Security](#-security)
- [Project Engagement](#-project-engagement)
- [License](#-license)

## ✨ Capabilities

- Detects the current public IP address of the machine
- Sends an authenticated `PUT` request to the Dynamic DNS Updater API
- Updates the DNS record for a given domain using a selected DNS provider
- Keeps DNS records in sync when your external IP changes

## 🚀 Usage

The application reads configuration from `appsettings.json` and accepts domain name and provider name as command-line arguments.

### CLI Options

Run with `dotnet run`:

```bash
dotnet run -- -d your-domain.example.com -p your-provider
```

Or run the built binary:

```bash
./DynamicDnsUpdater.Client -d your-domain.example.com -p your-provider
```

| Option | Value | Default | Description |
|--------|-------|---------|-------------|
| `-d`, `--dn`, `--domain`, `--domain-name` | `<string>` | — | Domain name to update |
| `-p`, `--pn`, `--provider`, `--provider-name` | `<string>` | — | DNS provider name |

### Example

```bash
dotnet run -- --domain-name example.com --provider-name cloudflare
```

## 📦 Installation

[![Obtain it from GitHub](https://raw.githubusercontent.com/hmlendea/readme-assets/master/badges/stores/github.png)](https://github.com/hmlendea/dynamic-dns-updater-client/releases)

Download the latest release from [GitHub Releases](https://github.com/hmlendea/dynamic-dns-updater-client/releases).

## ⚙️ Configuration

### Configuration Files

| File | Scope | Purpose |
|------|-------|---------|
| `appsettings.json` | Application | API connection settings |

### Settings

The subsequent settings are recognised:

| Section | Key | Type | Default | Required | Description |
|---------|-----|------|---------|----------|-------------|
| `apiSettings` | `baseUrl` | `string` | — | Yes | Base URL of the Dynamic DNS Updater API |
| `apiSettings` | `apiKey` | `string` | — | Yes | Bearer token used to authenticate requests |

### Command-Line Options

| Option | Value | Default | Description |
|--------|-------|---------|-------------|
| `-d`, `--dn`, `--domain`, `--domain-name` | `<string>` | — | Domain name to update |
| `-p`, `--pn`, `--provider`, `--provider-name` | `<string>` | — | DNS provider name |

## 🛠️ Development

### Requirements

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Setup

```bash
git clone https://github.com/hmlendea/dynamic-dns-updater-client.git
cd dynamic-dns-updater-client
dotnet restore
```

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run --project DynamicDnsUpdater.Client -- --domain-name example.com --provider-name cloudflare
```

### Test

```bash
dotnet test
```

### Release

The repository includes `release.sh`, which delegates to the upstream deployment script used by the project maintainer.

```bash
bash ./release.sh 1.0.0
```

This script downloads and executes an external release helper from `https://raw.githubusercontent.com/hmlendea/dotnet-release-scripts/master/release/dotnet/10.0.sh`.

**Note:** Piping into `bash` is an intensely controversial topic. Please review any external scripts before running them in your environment!

## ⚙️ GitHub Actions

| Workflow | Purpose | What it does |
|----------|---------|--------------|
| [ dotnet.yml ](https://github.com/hmlendea/dynamic-dns-updater-client/blob/master/.github/workflows/dotnet.yml) | Continuous integration | Restores dependencies, builds the solution, and runs tests on every push and pull request to `master` |

## 🗂️ Project Structure

### Projects and Packages

| Project | Type | Purpose |
|---------|------|---------|
| `DynamicDnsUpdater.Client` | Console Application | Main CLI application |
| `DynamicDnsUpdater.Client.UnitTests` | Unit Test Project | Unit tests for the client |

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

This project is being distributed under the `GNU General Public License v3.0 or later`.
See [LICENSE](./LICENSE) for additional information.
