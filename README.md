# Simple Calendar

**Simple Calendar** is a simple, light-weight calendar that allows users to view and manage events.

## Features

- View the calendar for any given month.
- Add, edit, or delete events.
- Display events in a daily or monthly view.
- Responsive and lightweight UI.

## Tech Stack

This project is built using the following technologies:

- **Backend**: C# (ASP.NET Core)
- **Frontend**: HTML, CSS, JavaScript
- **Database**: SQL Server

## Installation

To get started with the project locally, follow these steps:

### Prerequisites

- .NET Core SDK (version 8 or higher)

### Setup

1. Clone the repository:

    ```bash
    git clone https://github.com/Rhurac1986/cvp_waldeinsamkeit.git
    cd cvp_waldeinsamkeit
    ```

2. Install the necessary dependencies:

    - For **backend** (C#):
        ```bash
        cd EinfacherKalender/EinfacherKalender
        dotnet restore
        ```

3. Add the Calendar API Client ID and Secret:

    - Edit the **.env** file:
        ```ASPNETCORE_ENVIRONMENT=Development
        CLIENT_ID={calendar_api_client_id}
        CLIENT_SECRET={calendar_api_client_secret}
        ```

4. Run the backend server:

    ```bash
    dotnet run
    ```

    This will start the server, usually at `http://localhost:5000`.

5. Open the frontend in a browser.

## Usage

1. Open the calendar in your browser.
2. Navigate through months using the arrows.
3. Select a day to display its events in the event list.
4. Add a new event by double-clicking a selected day.
5. Add, edit, or delete events by interacting with the event list.
6. Events are saved in a third party API.
7. Users and the events they own are saved locally in the connected database.

## Contributing

We welcome contributions to the Simple Calendar project! To contribute:

1. Fork the repository.
2. Create a new branch (`git checkout -b feature/your-feature`).
3. Make your changes and commit them (`git commit -am 'Add new feature'`).
4. Push to your branch (`git push origin feature/your-feature`).
5. Create a new Pull Request.

Please make sure to follow the [Code of Conduct](CODE_OF_CONDUCT.md) and [Contributing Guidelines](CONTRIBUTING.md) before submitting your PR.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- https://fullcalendar.io

---
