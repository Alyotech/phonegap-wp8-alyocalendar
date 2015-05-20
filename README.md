# phonegap-wp8-alyocalendar
Ouverture de l'agenda d'un mobile Windows Phone et ajout d'une nouvelle tâche.


Git: https://github.com/Alyotech/phonegap-wp8-alyocalendar.git

Dependency:

   "Json.Net" NuGet Package

Feature:

   <feature name="Calendar">
        <param name="wp-package" value="calendar"/>
   </feature>
   
Implementation:

   /*  Direct call
    *
    *  string: subject
    *  string: location
    *  string: notes
    *  string: startDate (string ISO date)
    *  string: endDate (string ISO date)
    *  function: onSuccess (not implemented)
    *  function: onError
    */
   window.plugins.calendar.createEvent(subject, location, notes, startDate, endDate, onSuccess, onError);

   /* Call from 'Candendar.js' wrapper */
    cordova.exec(successCallback, errorCallback, "Calendar", "createEventWithOptions", [{
      "title": title,
      "location": location,
      "notes": notes,
      "startTime": startDate instanceof Date ? startDate.getTime() : null,
      "endTime": endDate instanceof Date ? endDate.getTime() : null,
      "options": mergedOptions
   }]);
    
    
    
