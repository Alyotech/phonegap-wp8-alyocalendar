# phonegap-wp8-alyocalendar
Ouverture de l'agenda d'un mobile Windows Phone et ajout d'une nouvelle tâche.


Git: https://github.com/Alyotech/phonegap-wp8-alyocalendar.git

Dependency:

   <b>"Json.Net"</b> NuGet Package

Feature:
<pre>
   &lt;feature name="Calendar"&gt;
        &lt;param name="wp-package" value="calendar"/&gt;
   &lt;/feature&gt;
</pre> 

Implementation:
<pre>
   /*  Direct call from JS
    *
    *  @param {string} subject
    *  @param {string} location
    *  @param {string} notes
    *  @param {string} startDate (string ISO date)
    *  @param {string} endDate (string ISO date)
    *  @param {function} onSuccess (not implemented)
    *  @param {function} onError
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
</pre>
