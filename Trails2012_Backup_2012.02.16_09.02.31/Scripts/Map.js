// Andy - this is some code to allow user to add one (and only one) marker to the map, 
// and to double-click on he marker to remove it

var map;
var markersArray = [];

function initializeMap(latitude, longitude, isreadonly) {

    if (latitude != "" && longitude != "") {
        var latlng = new google.maps.LatLng(latitude, longitude);
        var zoom = 13;
    }
    else {
        var latlng = new google.maps.LatLng(51, -115.5);
        var zoom = 9;
    }

    var options = { zoom: zoom, center: latlng,
        mapTypeId: google.maps.MapTypeId.TERRAIN
    };
    var map = new google.maps.Map(document.getElementById
			("map_canvas"), options);

    if (latitude != "" && longitude != "")
        placeMarker(latlng, map, isreadonly);

    if (isreadonly != "True") {
        google.maps.event.addListener(map, 'click', function(e) {
            placeMarker(e.latLng, map, isreadonly);
        });
    }
}

function placeMarker(position, map, isreadonly) {
    var marker = new google.maps.Marker({
        position: position,
        map: map
    });
    removeMarkers();
    markersArray.push(marker);
    $("#Latitude").val(position.lat());
    $("#Longitude").val(position.lng());

    if (isreadonly != "True") {
        google.maps.event.addListener(marker, 'dblclick', function(point, source, overlay) {
            marker.setMap(null);
            $("#Latitude").val("");
            $("#Longitude").val("");
        });
    }
    //map.panTo(position);
}

// Removes the overlays from the map, but keeps them in the array 
function removeMarkers() {
    if (markersArray) {
        for (i in markersArray) {
            markersArray[i].setMap(null);
            markersArray.splice(i);
        }
        $("#Latitude").val("");
        $("#Longitude").val("");
    }
}

