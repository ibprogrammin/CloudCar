var map = null;
var geocoder = null;

function initialize() {
    if (GBrowserIsCompatible()) {
        map = new GMap2(document.getElementById("map_canvas"));

        var mapTypeControl = new GMapTypeControl();
        var topRight = new GControlPosition(G_ANCHOR_TOP_RIGHT, new GSize(10, 10));
        map.addControl(mapTypeControl, topRight);
        map.addControl(new GSmallMapControl());

        geocoder = new GClientGeocoder();
    }
}

function showAddress(address, title) {
    if (geocoder) {
        geocoder.getLatLng(
      address,
      function(point) {
          if (!point) {
              //alert(address + " not found");
          } else {
              map.setCenter(point, 15);
              var marker = new GMarker(point);
              map.addOverlay(marker);
              marker.openInfoWindowHtml("<h4>" + title + "</h4>");
          }
      }
        );
    }
}

function showDirections() {
    getDirections(document.getElementById("Address").value);
}

function getDirections(address) {
    directionsPanel = document.getElementById("mapdirections");
    directions = new GDirections(map, directionsPanel);
    directions.load("from: " + address + " to: 1180 Stone Church Rd. East, Hamilton, On, L8W 2C7, Canada");
}