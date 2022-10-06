var selectedCountryAndCurrency = getSelectedCountryAndCurrency();
var PageCountryId = selectedCountryAndCurrency.countryId;
var centerlat = 38, centerlon = -98.35, zoomarea = 15;
if (PageCountryId == 2) { //canada
    centerlat = 56.14;
    centerlon = -106.34;
}
var map;
var marker;

$(document).ready(function () {
    initMap();
});

function initMap() {
    google.maps.visualRefresh = true;
    LoadMap(zoomarea, centerlat, centerlon);
    LoadMapData();
}

function LoadMap(zoomArea, lat, long) {
    map = new google.maps.Map(document.getElementById('job-map'), {
        zoom: zoomArea,
        center: { lat: lat, lng: long },
        mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
    });

    return map;
}

function LoadMapData() {
    var latitude = parseFloat($(".latitude").val());
    var longitude = parseFloat($(".longitude").val());

    if (latitude != 0 && longitude != 0) {
        centerlat = latitude;
        centerlon = longitude;
    }

    getMapMarker(centerlat, centerlon);
    map.setCenter({
        lat: centerlat,
        lng: centerlon
    });
}

function getMapMarker(latitude, longitude) {
    if (marker != null) {
        marker.setMap(null);
    }

    marker = new google.maps.Marker({
        'position': new google.maps.LatLng(latitude, longitude),
        map: map,
        draggable: true
    });
    // Make the marker-pin blue!
    marker.setIcon('https://maps.google.com/mapfiles/ms/icons/blue-dot.png');

    marker.addListener('dragend', function (evt) {
        var curLat = evt.latLng.lat().toFixed(8);
        var curLong = evt.latLng.lng().toFixed(8);
        $(".latitude").val(curLat);
        $(".longitude").val(curLong);
        $(".latitude").trigger('blur');
    });
}

function refreshJobMap() {
    LoadMapData();
}