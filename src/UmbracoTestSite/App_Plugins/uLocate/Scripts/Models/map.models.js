(function (models, undefined) {

    models.MapControlOptions = function(data) {
        var self = this;
        if (data == undefined) {
            self.position = '';
            self.style = '';
        } else {
            if (data.position) {
                self.position = data.position;
            }
            if (data.style) {
                self.style = data.style;
            }
        }
    };

    /**
     * @ngdoc method
     * @name MapOptions
     * @function
     * 
     * @description - A Google maps options object.
     */
    models.MapOptions = function(data) {
        var self = this;
        if (data === undefined) {
            self.center = new google.maps.LatLng(0, 0);
            self.zoom = 15;
        } else {
            self.center = new google.maps.LatLng(data.center.latitude, data.center.longitude);
            self.zoom = data.zoom;
            if ((typeof data.mapTypeControl) !== 'undefined') {
                self.mapTypeControl = data.mapTypeControl;
            }
            if (data.mapTypeControlOptions) {
                self.mapTypeControlOptions = new uLocate.Models.MapControlOptions(data.mapTypeControlOptions);
            }
            if ((typeof data.panControl) !== 'undefined') {
                self.panControl = data.panControl;
            }
            if (data.panControlOptions) {
                self.panControlOptions = new uLocate.Models.MapControlOptions(data.panControlOptions);
            }
            if ((typeof data.scaleControl) !== 'undefined') {
                self.scaleControl = data.scaleControl;
            }
            if ((typeof data.streetViewControl) !== 'undefined') {
                self.streetViewControl = data.streetViewControl;
            }
            if (data.streetViewControlOptions) {
                self.streetViewControlOptions = new uLocate.Models.MapControlOptions(data.streetViewControlOptions);
            }
            if ((typeof data.zoomControl) !== 'undefined') {
                self.zoomControl = data.zoomControl;
            }
            if (data.zoomControlOptions) {
                self.zoomControlOptions = new uLocate.Models.MapControlOptions(data.zoomControlOptions);
            }
            if (data.styles) {
                self.styles = _.map(data.styles, function (style) {
                    return new uLocate.Models.MapStyle(style);
                });
            }

        };
    };

    models.MapMarker = function(data) {
        var self = this;
        if (data === undefined) {
            self.position = new google.maps.LatLng(0, 0);
        } else {
            self.map = data.map;
            if (data.icon) {
                self.icon = data.icon;
            }
            if (data.position instanceof google.maps.LatLng) {
                self.position = data.position;
            } else {
                self.position = new google.maps.LatLng(data.position[0], data.position[1]);
            }
            if (data.title) {
                self.title = data.title;
            }
        }
    };

    models.MapStyle = function (data) {
        var self = this;
        if (data === undefined) {
            self.featureType = '';
            self.stylers = {};
        } else {
            self.featureType = data.featureType;
            self.stylers = data.stylers;
        }
    };

    models.MapView = function(data) {
        var self = this;
        if (data === undefined) {
            self.coordinates = [0, 0];
            self.smoothAnimation = false;
            self.zoom = 15;
        } else {
            self.coordinates = data.coordinates;
            self.smoothAnimation = data.smoothAnimation;
            self.zoom = data.zoom;
        }
    }

    models.MarkerSymbolIcon = function(data) {
        var self = this;
        if (data == undefined) {
            self.path = '';
            self.fillColor = '#000000';
            self.fillOpacity = 1;
            self.strokeColor = '';
            self.strokeWeight = 0;
            self.scale = 1 / 2;
        } else {
            self.path = data.path;
            self.fillColor = data.fillColor;
            self.fillOpacity = data.fillOpacity;
            self.strokeColor = data.strokeColor;
            self.strokeWeight = data.strokeWeight;
            self.scale = data.scale;
        }
    };

}(window.uLocate.Models = window.uLocate.Models || {}));