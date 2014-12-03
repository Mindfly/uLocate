(function (constants, undefined) {

    constants.MAP_STYLES = [
        {
            "featureType": "landscape",
            "stylers": [
                { "visibility": "on" },
                { "color": "#ffffff" }
            ]
        }, {
            "featureType": "road",
            "elementType": "geometry",
            "stylers": [
                { "visibility": "on" },
                { "color": "#d9d9d9" }
            ]
        }, {
            "featureType": "poi.attraction",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "featureType": "poi.business",
            "stylers": [
                { "visibility": "on" },
                { "color": "#d9d9d9" }
            ]
        }, {
            "featureType": "poi.government",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "featureType": "poi.medical",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "featureType": "poi.park",
            "stylers": [
                { "visibility": "on" },
                { "color": "#53a93f" },
                { "lightness": 37 }
            ]
        }, {
            "featureType": "poi.place_of_worship",
            "stylers": [
                { "visibility": "off" }
            ]
        }, {
            "featureType": "poi.school",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "featureType": "poi.sports_complex",
            "stylers": [
                { "visibility": "off" }
            ]
        }, {
            "featureType": "water",
            "stylers": [
                { "visibility": "on" },
                { "color": "#049cdb" }
            ]
        }, {
            "featureType": "poi.business",
            "elementType": "labels"
        }, {

        }, {
            "featureType": "administrative",
            "stylers": [
                { "visibility": "on" },
                { "color": "#f8f8f8" }
            ]
        }, {
            "elementType": "labels.text.fill",
            "stylers": [
                { "visibility": "on" },
                { "color": "#1d1d1d" }
            ]
        }, {
            "elementType": "labels.text.stroke",
            "stylers": [
                { "visibility": "on" },
                { "color": "#ffffff" }
            ]
        }, {

        }
    ];

    constants.MARKER_ICON = {
        path: 'M0-165c-27.618 0-50 21.966-50 49.054C-50-88.849 0 0 0 0s50-88.849 50-115.946C50-143.034 27.605-165 0-165z',
        fillColor: '#ff7100',
        fillOpacity: 1,
        strokeColor: '#000000',
        strokeWeight: 2,
        scale: 1 / 4
    };

}(window.uLocate.Constants = window.uLocate.Constants || {}));