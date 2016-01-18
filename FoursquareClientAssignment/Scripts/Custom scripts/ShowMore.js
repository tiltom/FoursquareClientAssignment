(function() {


    //code dealing with product index page loadMore ajaxCall and smooth scroll down
    $(function() {

        var loadCount = 1;
        $("#showMore").on("click", function(e) {

            e.preventDefault();

            var url = "/Home/ShowMoreVenues/";
            var venueQuery = $("#query").val();

            $.ajax({
                url: url,
                data: { size: loadCount * 10, venueQuery: venueQuery },
                cache: false,
                type: "POST",
                success: function(data) {

                    if (data.length !== 0) {
                        $(data.ModelString).insertBefore("#endOfTable").hide().fadeIn(1000);
                    }

                    var ajaxModelCount = data.ModelCount - (loadCount * 10);
                    if (ajaxModelCount <= 0) {
                        $("#showMore").hide().fadeOut(1000);
                    }

                },
                error: function(xhr, status, error) {
                    console.log(xhr.responseText);
                    alert("message : \n" + "An error occurred, for more info check the js console" + "\n status : \n" + status + " \n error : \n" + error);
                }
            });

            loadCount = loadCount + 1;

        });

    });


})();