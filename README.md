# internship

## Introduction
This program matches locations to their respective regions based on their coordinates. The program reads two JSON files as input from App_Data folder, one containing locations and the other containing regions, and generates a JSON file as output into App_Data folder, which lists all regions and the locations that fall within those regions.

## Prerequisites
- .NET 7.0

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
