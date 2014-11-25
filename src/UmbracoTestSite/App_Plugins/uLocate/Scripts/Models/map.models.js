(function (models, undefined) {

    models.MapOptions = function(data) {
        var self = this;
        if (data === undefined) {
            self.center = new google.MapService.latLng(0, 0);
            self.zoom = 15;
        } else {
            self.center = new google.MapService.latLng(data.center.latitude, data.center.longitude);
            if(self.styles){
                self.styles = _.map(data.styles, function (style) {
                    return new uLocate.Models.MapStyle(style);
                });
            }
            self.zoom = data.zoom;
        };
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

}(window.uLocate.Models = window.uLocate.Models || {}));