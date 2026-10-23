# Privacy and Personal Data

This document describes how Dynamic DNS Updater Client at https://github.com/hmlendea/dynamic-dns-updater-client handles personal data. It covers the application behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 📑 Table of Contents

- [What This Document Covers](#what-this-document-covers)
- [Self-Hosted Deployments](#self-hosted-deployments)
- [Data We Handle](#data-we-handle)
  - [Data Provided to the Application](#data-provided-to-the-application)
  - [Data Generated or Collected by the Application](#data-generated-or-collected-by-the-application)
  - [Data Received from Integrations](#data-received-from-integrations)
- [Processing and Use](#processing-and-use)
- [Storage, Retention, and Deletion](#storage-retention-and-deletion)
- [External Processing and Integrations](#external-processing-and-integrations)
- [User Controls and Requests](#user-controls-and-requests)
- [International Transfers](#international-transfers)
- [Data Protection and Security](#data-protection-and-security)
- [Document Changes](#document-changes)
- [Contact](#contact)

## 🔎 What This Document Covers

This document describes how Dynamic DNS Updater Client at https://github.com/hmlendea/dynamic-dns-updater-client handles personal data. It covers the application behaviour and verified integrations described below. Where the software is self-hosted, the instance operator may have separate responsibilities described below.

## 🏠 Self-Hosted Deployments

This project is a command-line application that users download, build, and run on their own infrastructure. The project maintainers do not operate a hosted service. Each operator controls their instance's configuration, local storage, logs, backups, access controls, retention, and request handling.

The application sends the following data from a self-hosted instance to the configured Dynamic DNS Updater API:
- Domain name
- Provider name
- Detected public IP address
- API key (as Bearer token in Authorization header)

Operators can disable this flow by not running the application or by configuring a local API endpoint.

## 📥 Data We Handle

### Data Provided to the Application

- Domain name (provided via CLI arguments)
- Provider name (provided via CLI arguments)
- API base URL (provided via appsettings.json configuration file)
- API key / Bearer token (provided via appsettings.json configuration file)

### Data Generated or Collected by the Application

- Public IP address (detected automatically by querying a public IP detection service)
- Application logs via NuciLog including: operation start, success, and failure states; domain name; provider name; API base URL; detected public IP; API response code and message (on failure)

### Data Received from Integrations

- No personal data is received from integrations or third parties. The application only sends data to the configured Dynamic DNS Updater API.

## 🧭 Processing and Use

The application processes the data described above for these verified functions:
- Detecting the current public IP address of the machine — Public IP address
- Sending an authenticated PUT request to the Dynamic DNS Updater API — Domain name, Provider name, API key, Public IP address
- Updating the DNS record for a given domain using a selected DNS provider — Domain name, Provider name, Public IP address
- Logging operation state for debugging and monitoring — Domain name, Provider name, API base URL, Public IP address, API response

## 🗄️ Storage, Retention, and Deletion

The application does not persist personal data locally. It operates as a stateless CLI tool:
- Configuration (API base URL, API key) is read from appsettings.json at runtime and held in memory only
- CLI arguments (domain name, provider name) are held in memory only
- Detected public IP address is held in memory only
- Logs are written to standard output/error via NuciLog and are not persisted by the application itself
- No local databases, files, caches, or backups of personal data are created by the application

For self-hosted deployments, the instance operator controls any log retention, backup, or storage configured at the operating system or container level.

## 🔗 External Processing and Integrations

| Service or integration | Purpose | Data involved | Configuration or documentation |
|-----------------------|---------|---------------|--------------------------------|
| Dynamic DNS Updater API | Update DNS records | Domain name, Provider name, Public IP address, API key (Bearer token) | Configured via appsettings.json `apiSettings.baseUrl` and `apiSettings.apiKey` |
| Public IP detection service | Detect current public IP | Public IP address | Built-in; queries a public endpoint |
| NuciLog | Application logging | Domain name, Provider name, API base URL, Public IP address, API response | Configured via appsettings.json `NuciLoggerSettings` |

## ⚙️ User Controls and Requests

The following documented controls are available:
- Configuration via appsettings.json — Operator controls API base URL and API key
- CLI arguments — Operator controls domain name and provider name per invocation
- Log output — Operator controls log destination and verbosity via NuciLoggerSettings in appsettings.json
- No built-in data access, rectification, or deletion requests — The application does not store personal data

## 🌍 International Transfers

Data is sent to the Dynamic DNS Updater API endpoint configured by the operator via `apiSettings.baseUrl`. The geographic location of the API endpoint is controlled by the operator. The public IP detection service may be located in a different jurisdiction.

## 🛡️ Data Protection and Security

- API key is transmitted as a Bearer token in the Authorization header over HTTPS (when the configured API base URL uses HTTPS)
- No personal data is encrypted at rest because the application does not persist data
- No secrets are logged — NuciLog configuration should exclude sensitive fields
- For self-hosted deployments, the operator is responsible for:
  - Securing the appsettings.json file containing the API key
  - Ensuring the API endpoint uses HTTPS
  - Managing log access, retention, and protection
  - Network exposure of the machine running the application
  - Keeping the application and dependencies updated

The original maintainers are not responsible for changes, modifications, or privacy issues introduced by downstream forks.

## 🔄 Document Changes

Update this document when application data flows, storage, integrations, or deployment responsibilities change. The current version is published at https://github.com/hmlendea/dynamic-dns-updater-client/blob/master/PRIVACY.md.

## 📬 Contact

For questions about application data handling, contact the project maintainers via GitHub issues at https://github.com/hmlendea/dynamic-dns-updater-client/issues. For a self-hosted instance, contact the instance operator, unless the project explicitly handles the request. Include the application version and deployment context; do not send passwords, access tokens, or other secrets.