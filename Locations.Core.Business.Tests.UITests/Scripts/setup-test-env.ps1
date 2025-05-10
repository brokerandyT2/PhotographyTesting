# setup-test-env.ps1
# Script to set up UI testing environment for MAUI Android application
# Copyright 2025 - Your Company

# This script requires Administrator privileges to run
if (-NOT ([Security.Principal.WindowsPrincipal][Security.Principal.WindowsIdentity]::GetCurrent()).IsInRole([Security.Principal.WindowsBuiltInRole] "Administrator")) {
    Write-Warning "Please run this script as Administrator!"
    exit
}

# Create output directory for logs
$logsDir = Join-Path $PSScriptRoot "logs"
if (-not (Test-Path $logsDir)) {
    New-Item -ItemType Directory -Path $logsDir | Out-Null
    Write-Host "Created logs directory: $logsDir" -ForegroundColor Green
}

# Create output directory for screenshots
$screenshotsDir = Join-Path $PSScriptRoot "Screenshots"
if (-not (Test-Path $screenshotsDir)) {
    New-Item -ItemType Directory -Path $screenshotsDir | Out-Null
    Write-Host "Created screenshots directory: $screenshotsDir" -ForegroundColor Green
}

# Check if Node.js is installed
try {
    $nodeVersion = node -v
    Write-Host "Node.js is installed: $nodeVersion" -ForegroundColor Green
}
catch {
    Write-Warning "Node.js is not installed. Please install Node.js from https://nodejs.org/"
    exit
}

# Check if Appium is installed
try {
    $appiumVersion = appium -v
    Write-Host "Appium is installed: $appiumVersion" -ForegroundColor Green
}
catch {
    Write-Host "Appium is not installed. Installing Appium..." -ForegroundColor Yellow
    npm install -g appium
    
    if ($LASTEXITCODE -ne 0) {
        Write-Warning "Failed to install Appium. Please install it manually."
        exit
    }
    
    Write-Host "Appium installed successfully!" -ForegroundColor Green
}

# Install Appium UiAutomator2 driver for Android
try {
    $driverList = appium driver list
    if ($driverList -match "uiautomator2") {
        Write-Host "UiAutomator2 driver is already installed" -ForegroundColor Green
    } else {
        Write-Host "Installing UiAutomator2 driver..." -ForegroundColor Yellow
        appium driver install uiautomator2
        Write-Host "UiAutomator2 driver installed successfully!" -ForegroundColor Green
    }
}
catch {
    Write-Warning "Failed to install UiAutomator2 driver: $_"
}

# Check if Android SDK is installed
$androidHome = $env:ANDROID_HOME
if (-not $androidHome) {
    $androidHome = $env:ANDROID_SDK_ROOT
}

if (-not $androidHome) {
    Write-Warning "Android SDK environment variables (ANDROID_HOME or ANDROID_SDK_ROOT) not found."
    $defaultAndroidSDK = "$env:LOCALAPPDATA\Android\Sdk"
    
    if (Test-Path $defaultAndroidSDK) {
        Write-Host "Found Android SDK at default location: $defaultAndroidSDK" -ForegroundColor Yellow
        Write-Host "Setting ANDROID_HOME environment variable..." -ForegroundColor Yellow
        [System.Environment]::SetEnvironmentVariable("ANDROID_HOME", $defaultAndroidSDK, [System.EnvironmentVariableTarget]::Machine)
        $env:ANDROID_HOME = $defaultAndroidSDK
        Write-Host "ANDROID_HOME environment variable set to: $defaultAndroidSDK" -ForegroundColor Green
    } else {
        Write-Warning "Android SDK not found. Please install Android SDK and set ANDROID_HOME environment variable."
    }
} else {
    Write-Host "Android SDK found at: $androidHome" -ForegroundColor Green
}

# Check if Android emulator is installed
if ($androidHome) {
    $emulatorPath = Join-Path $androidHome "emulator\emulator.exe"
    if (Test-Path $emulatorPath) {
        Write-Host "Android emulator found at: $emulatorPath" -ForegroundColor Green
        
        # List available emulators
        Write-Host "Available emulators:" -ForegroundColor Yellow
        & "$emulatorPath" -list-avds
    } else {
        Write-Warning "Android emulator not found. Please install it using Android Studio."
    }
}

# Check if .NET SDK is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host ".NET SDK is installed: $dotnetVersion" -ForegroundColor Green
}
catch {
    Write-Warning ".NET SDK is not installed. Please install it from https://dotnet.microsoft.com/download"
    exit
}

# Install required NuGet packages
Write-Host "Installing NuGet packages..." -ForegroundColor Yellow
$projectPath = Join-Path $PSScriptRoot "Locations.Core.Business.Tests.UITests.csproj"
if (Test-Path $projectPath) {
    dotnet restore $projectPath
    Write-Host "NuGet packages restored successfully!" -ForegroundColor Green
} else {
    Write-Warning "Project file not found at: $projectPath"
}

# Update appium.config.json with correct path to Android APK
$apkPath = ""
$defaultAppDir = Join-Path $PSScriptRoot "..\..\Location.Core\bin\Debug\net9.0-android"
if (Test-Path $defaultAppDir) {
    $apkFiles = Get-ChildItem -Path $defaultAppDir -Filter "*.apk" -Recurse
    if ($apkFiles.Count -gt 0) {
        $apkPath = $apkFiles[0].FullName
        Write-Host "Found APK at: $apkPath" -ForegroundColor Green
        
        # Update appium.config.json
        $appiumConfigPath = Join-Path $PSScriptRoot "appium.config.json"
        if (Test-Path $appiumConfigPath) {
            $config = Get-Content $appiumConfigPath -Raw | ConvertFrom-Json
            $config.android.appPath = $apkPath.Replace("\", "\\")
            $config | ConvertTo-Json -Depth 10 | Set-Content $appiumConfigPath
            Write-Host "Updated appium.config.json with APK path" -ForegroundColor Green
        }
    } else {
        Write-Warning "No APK files found in the output directory. Please build the app first."
    }
} else {
    Write-Warning "App output directory not found. Please build the app first."
}

Write-Host "UI Test environment setup completed!" -ForegroundColor Green
Write-Host "To run tests, use: dotnet test $projectPath" -ForegroundColor Yellow
Write-Host "To run specific domain tests, use: dotnet run --project $projectPath -- --domain {DomainName}" -ForegroundColor Yellow