$(document).ready(function () {

    $('#retrieve-resources').click(function () {
        var displayResources = $('#display-resources');

        displayResources.text('Loading data from JSON source...');
        var url = "https://www.omdbapi.com/?apikey=aa55f565&t=".concat(document.getElementById('title').value).concat('&plot=full').concat('&y=').concat(document.getElementById('year').value);

        $('form :input').val('');


        $.ajax({
           
            url: url,
            type: "GET",
            dataType: "json",
            success: function (result) {
                if (result.hasOwnProperty('Error')) {
                    displayResources.html(result.Error);
                    return;
                }
                //release-date formating
                var input = result.Released;
                var year = input.slice(-4),
                    month = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                        'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'].indexOf(input.substr(3, 3)) + 1,
                    day = input.substr(0, 2);
                var output = year + '-' + (month < 10 ? '0' : '') + month + '-' + day;

                var array = result.Genre.replace(new RegExp("\\s", "g"), '').split(',');
                $("#categories").val(array);
                $("#title").val(result.Title);
                $("#release-date").val(output);
                $("#duration").val(result.Runtime.match(new RegExp("\\d+")));
                $("#writer").val(result.Writer.replace(new RegExp("\\s?\\(.*?\\)", "g"), ''));
                $("#director").val(result.Director.replace(new RegExp("\\s?\\(.*?\\)", "g"), ''));
                $("#actors").val(result.Actors.replace(new RegExp("\\s?\\(.*?\\)", "g"), ''));
                $("#year").val(result.Year);
                $("#plot").val(result.Plot);
                $("#poster").val(result.Poster);
                displayResources.html('Form is filled!');
            }
           
        });

        
    });
});