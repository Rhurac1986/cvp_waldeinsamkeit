document.addEventListener('DOMContentLoaded', function() {
	(async function() {
		// Get calendar events from the server
		const getEventsAsync = async () => {
			try {
				const response = await fetch('Index?handler=Events');
				if (!response.ok) {
					console.error(`An error occurred. Status: ${response.status}`);
					return [];
				}
				return await response.json();
			} catch(error) {
				console.error('An error occurred', error);
				return [];
			}
		}

		// Display the list of events for the selected day
		const displayEventsList = (selectedDate, isDaySelected) => {
			// Create event list container
			const eventListContainer = document.getElementById('event-list');
			const titleContainer = document.getElementById('event-list-title');
			eventListContainer.innerHTML = ''; // Clear previous list

			// Attach event list to container
			const eventList = document.createElement('ul');
			eventList.classList.add('list-group');
			eventListContainer.appendChild(eventList);

			// Add events to the event list
			let filteredEvents;
			if (isDaySelected) {
				// Update title for selected day
				const [year, month, day] = selectedDate.split('-').map(Number);
				const selectedDateObj = new Date(year, month - 1, day);
				const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
				const formattedDate = selectedDateObj.toLocaleDateString('en-US', options);
				titleContainer.innerHTML = `Events for ${formattedDate}`;

				// Display events for the selected day
				const normalizeDate = (date) => { return new Date(date).setHours(0, 0, 0, 0); }
				filteredEvents = events.filter(event => normalizeDate(event.startDate) <= normalizeDate(selectedDateObj) && normalizeDate(event.endDate) >= normalizeDate(selectedDateObj));
				if (filteredEvents.length === 0) {
					eventListContainer.innerHTML = '<p>No events for this day.</p>';
				} else {
					filteredEvents.forEach(function (event) {
						const eventItem = document.createElement('li');
						eventItem.classList.add('list-group-item', 'list-group-item-light', 'd-flex', 'justify-content-between', 'align-items-center');
						eventItem.innerHTML = `
							<span>
								<strong>${event.title}</strong><br>
								<em>${event.description}</em>
							</span>
							<a href="#" class="view-event" data-title="${event.title}" data-description="${event.description}" data-start="${event.startDate}" data-end="${event.endDate}">
								<i class="fas fa-eye"></i>
							</a>`;
						eventList.appendChild(eventItem);
					});
				}

				attachViewEventClickHandlers();
			}
		}

		// Show modal with event details
		const showModal = (title, description, start, end) => {
			const modalTitle = document.querySelector('#modalCenter .modal-title');
			const modalBody = document.querySelector('#modalCenter .modal-body');
			modalTitle.textContent = title;
			modalBody.innerHTML = `<p>${description}</p>
			<p>Start: ${new Date(start).toLocaleString()}</p>`;
			modalBody.innerHTML += end ? `<p>End: ${new Date(end).toLocaleString()}</p>` : '';
			$('#modalCenter').modal('show');
		}

		// Add click handler for the eyeball link
		const attachViewEventClickHandlers = () => {
			document.querySelectorAll('.view-event').forEach(link => {
				link.removeEventListener('click', handleViewEventClick);
				link.addEventListener('click', handleViewEventClick);
			});
		}

		// Handler a click on the view event link
		const handleViewEventClick = (event) => {
			event.preventDefault();
			const title = event.currentTarget.dataset.title;
			const description = event.currentTarget.dataset.description;
			const start = event.currentTarget.dataset.start;
			const end = event.currentTarget.dataset.end;
			showModal(title, description, start, end);
		}

		// Attach double click handlers to each day in the calendar
		const attachAddEventDoubleClickHandlers = () => {
			document.querySelectorAll('.fc-day').forEach((dayEl) => {
				dayEl.removeEventListener('dblclick', handleDayDoubleClick);
				dayEl.addEventListener('dblclick', handleDayDoubleClick);
			});
		}

		// Handle double click on a day in the calendar
		const handleDayDoubleClick = (event) => {
			const selectedDate = event.currentTarget.dataset.date;
			document.getElementById('startdate').value = selectedDate;
			document.getElementById('enddate').value = selectedDate;
			document.getElementById('starttime').value = '08:00';
			document.getElementById('endtime').value = '09:00';
			$('#eventModal').modal('show');
		};

		// Initialize FullCalendar.io calendar
		const calendarEl = document.getElementById('calendar');
		const events = await getEventsAsync();
		const calendar = new FullCalendar.Calendar(calendarEl, {
			initialView: 'dayGridMonth',
			locale: globals.language,  // Set the calendar language
			height: 'auto', // Automatically adjust to fit all days in the month
			windowResize: function () {
				calendar.updateSize(); // Resize the calendar when the window size changes
			},
			dateClick: function (info) {
				document.querySelectorAll('.fc-day').forEach(function (dayEl) {
					dayEl.style.backgroundColor = ''; // Clear previous selection
				});
				info.dayEl.style.backgroundColor = '#f0f0f0'; // Highlight selected day
				displayEventsList(info.dateStr, true); // Show events for selected day
			},
			datesSet: function (info) {
				// In latest version of FullCalendar.io, the datesSet event is triggered when dates change (e.g. when navigating to a different month)
				if (info.view.type === 'dayGridMonth') {
					displayEventsList(null, false); // Show events for selected month
				}
				attachAddEventDoubleClickHandlers();
			},
			eventClick: function (info) {
				showModal(info.event.title, info.event.extendedProps.description, info.event.start, info.event.end);
			},
			eventMouseEnter: function (mouseEnterInfo) {
				mouseEnterInfo.el.style.cursor = 'pointer';
			},
			eventMouseLeave: function (mouseLeaveInfo) {
				mouseLeaveInfo.el.style.cursor = '';
			},
			events: events.map(function (event) {
				return {
					title: event.title,
					description: event.description,
					start: event.startDate,
					end: event.endDate
				};
			})
		});

		// Simulate a click on a date in the calendar
		const triggerDateClick = (dateStr) => {
			calendar.trigger('dateClick', {
				dateStr: dateStr,
				dayEl: document.querySelector(`[data-date="${dateStr}"]`)
			});
		}

		calendar.render();
		attachAddEventDoubleClickHandlers();
		triggerDateClick(new Date().toLocaleDateString('en-CA'));
	})();
});
