# Internship

### Introduction

This program links locations to regions using their coordinates. It works by:

1. Reading two JSON files from the App_Data folder.
    -The first file defines locations.
    -The second file defines regions.

2. After reading these files, the program calculates which locations belong to which regions.

3. Finally, it creates a new JSON file in the App_Data folder.
    -This output file organizes all regions and shows which locations are part of them.

## Prerequisites
- .NET 6.0

## Installation
1. Clone the GitHub repository.
    ```bash
    git clone https://github.com/ugis90/internship.git
    ```
2. Navigate to the project directory.
    ```bash
    cd internship
    ```

## How to Compile and Run
1. Build the project.
    ```bash
    dotnet build
    ```
2. Run the application with the following command.
    ```bash
    dotnet run --regions=regions.json --locations=locations.json --output=results.json
    ```
