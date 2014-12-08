(function (models, undefined) {

    models.Property = function(data) {
        var self = this;
        if (data === undefined) {
            self.id = '';
            self.alias = '';
            self.name = '';
            self.type = '';
        } else {
            self.id = data.id;
            self.alias = data.alias;
            self.name = data.name;
            self.type = data.type;
        }
    }

    models.LocationType = function (data) {
        var self = this;
        if (data === undefined) {
            self.properties = [new uLocate.Models.Property()];
            self.name = '';
            self.icon = '';
            self.description = '';
        } else {
            self.properties = _.map(data.properties, function(property) {
                return new uLocate.Models.Property(property);
            });
            self.name = data.name;
            self.icon = data.icon;
            self.description = data.description;
        }
    };

}(window.uLocate.Models = window.uLocate.Models || {}));