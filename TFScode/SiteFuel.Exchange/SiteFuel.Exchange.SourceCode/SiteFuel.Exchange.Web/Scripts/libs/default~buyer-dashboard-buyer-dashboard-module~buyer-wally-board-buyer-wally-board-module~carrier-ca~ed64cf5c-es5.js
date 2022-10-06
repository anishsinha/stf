function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["default~buyer-dashboard-buyer-dashboard-module~buyer-wally-board-buyer-wally-board-module~carrier-ca~ed64cf5c"], {
  /***/
  "./node_modules/agm-direction/__ivy_ngcc__/fesm2015/agm-direction.js": function node_modulesAgmDirection__ivy_ngcc__Fesm2015AgmDirectionJs(module, __webpack_exports__, __webpack_require__) {
    "use strict";

    __webpack_require__.r(__webpack_exports__);
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "AgmDirectionModule", function () {
      return AgmDirectionModule;
    });
    /* harmony export (binding) */


    __webpack_require__.d(__webpack_exports__, "ɵa", function () {
      return AgmDirection;
    });
    /* harmony import */


    var _angular_core__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(
    /*! @angular/core */
    "./node_modules/@angular/core/__ivy_ngcc__/fesm2015/core.js");
    /* harmony import */


    var _agm_core__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(
    /*! @agm/core */
    "./node_modules/@agm/core/__ivy_ngcc__/fesm2015/agm-core.js");
    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,uselessCode} checked by tsc
     */


    var AgmDirection = /*#__PURE__*/function () {
      /**
       * @param {?} gmapsApi
       */
      function AgmDirection(gmapsApi) {
        _classCallCheck(this, AgmDirection);

        this.gmapsApi = gmapsApi; // Options

        this.travelMode = 'DRIVING';
        this.transitOptions = undefined;
        this.drivingOptions = undefined;
        this.waypoints = [];
        this.optimizeWaypoints = true;
        this.provideRouteAlternatives = false;
        this.avoidHighways = false;
        this.avoidTolls = false; // Remove or draw direction

        this.visible = true; // Direction change event handler

        this.onChange = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"](); // Direction response for the new request

        this.onResponse = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"](); // Send a custom infowindow

        this.sendInfoWindow = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"](); // Status of Directions Query (google.maps.DirectionsStatus.OVER_QUERY_LIMIT)

        this.status = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"](); // Marker drag event handler

        this.originDrag = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.destinationDrag = new _angular_core__WEBPACK_IMPORTED_MODULE_0__["EventEmitter"]();
        this.directionsService = undefined;
        this.directionsDisplay = undefined;
        this.waypointsMarker = []; // Use for visible flag

        this.isFirstChange = true;
      }
      /**
       * @return {?}
       */


      _createClass(AgmDirection, [{
        key: "ngOnInit",
        value: function ngOnInit() {
          if (this.visible === true) {
            this.directionDraw();
          }
        }
        /**
         * @param {?} obj
         * @return {?}
         */

      }, {
        key: "ngOnChanges",
        value: function ngOnChanges(obj) {
          /**
           * When visible is false then remove the direction layer
           */
          if (!this.visible) {
            try {
              this.removeMarkers();
              this.removeDirections();
            } catch (e) {}
          } else {
            if (this.isFirstChange) {
              /**
               * When visible is false at the first time
               */
              if (typeof this.directionsDisplay === 'undefined') {
                this.directionDraw();
              }

              this.isFirstChange = false;
              return;
            }
            /**
             * When renderOptions are not first change then reset the display
             */


            if (typeof obj.renderOptions !== 'undefined') {
              if (obj.renderOptions.firstChange === false) {
                this.removeMarkers();
                this.removeDirections();
              }
            }

            this.directionDraw();
          }
        }
        /**
         * @return {?}
         */

      }, {
        key: "ngOnDestroy",
        value: function ngOnDestroy() {
          this.destroyMarkers();
          this.removeDirections();
        }
        /**
         * This event is fired when the user creating or updating this direction
         * @return {?}
         */

      }, {
        key: "directionDraw",
        value: function directionDraw() {
          var _this = this;

          this.gmapsApi.getNativeMap().then(function (map) {
            if (typeof _this.directionsDisplay === 'undefined') {
              _this.directionsDisplay = new google.maps.DirectionsRenderer(_this.renderOptions);

              _this.directionsDisplay.setMap(map);

              _this.directionsDisplay.addListener('directions_changed', function () {
                _this.onChange.emit(_this.directionsDisplay.getDirections());
              });
            }

            if (typeof _this.directionsService === 'undefined') {
              _this.directionsService = new google.maps.DirectionsService();
            }

            if (typeof _this.panel === 'undefined') {
              _this.directionsDisplay.setPanel(null);
            } else {
              _this.directionsDisplay.setPanel(_this.panel);
            } // Render exist direction


            if (typeof _this.renderRoute === 'object' && _this.renderRoute !== null) {
              _this.directionsDisplay.setDirections(_this.renderRoute);

              _this.renderRoute = null; // or set undefined, ''
            } else {
              // Request new direction
              _this.directionsService.route({
                origin: _this.origin,
                destination: _this.destination,
                travelMode: _this.travelMode,
                transitOptions: _this.transitOptions,
                drivingOptions: _this.drivingOptions,
                waypoints: _this.waypoints,
                optimizeWaypoints: _this.optimizeWaypoints,
                provideRouteAlternatives: _this.provideRouteAlternatives,
                avoidHighways: _this.avoidHighways,
                avoidTolls: _this.avoidTolls
              }, function (response, status) {
                _this.onResponse.emit(response); // Emit Query Status


                _this.status.emit(status);
                /**
                 * DirectionsStatus
                 * https://developers.google.com/maps/documentation/javascript/directions#DirectionsStatus
                 */


                switch (status) {
                  case 'OK':
                    _this.directionsDisplay.setDirections(response);
                    /**
                     * Emit The DirectionsResult Object
                     * https://developers.google.com/maps/documentation/javascript/directions?hl=en#DirectionsResults
                     */
                    // Custom Markers


                    if (typeof _this.markerOptions !== 'undefined') {
                      _this.destroyMarkers(); // Set custom markers

                      /** @type {?} */


                      var _route = response.routes[0].legs[0];

                      try {
                        // Origin Marker
                        if (typeof _this.markerOptions.origin !== 'undefined') {
                          _this.markerOptions.origin.map = map;
                          _this.markerOptions.origin.position = _route.start_location;
                          _this.originMarker = _this.setMarker(map, _this.originMarker, _this.markerOptions.origin, _route.start_address);

                          if (_this.markerOptions.origin.draggable) {
                            _this.originMarker.addListener('dragend', function () {
                              _this.origin = _this.originMarker.position;

                              _this.directionDraw();

                              _this.originDrag.emit(_this.origin);
                            });
                          }
                        } // Destination Marker


                        if (typeof _this.markerOptions.destination !== 'undefined') {
                          _this.markerOptions.destination.map = map;
                          _this.markerOptions.destination.position = _route.end_location;
                          _this.destinationMarker = _this.setMarker(map, _this.destinationMarker, _this.markerOptions.destination, _route.end_address);

                          if (_this.markerOptions.destination.draggable) {
                            _this.destinationMarker.addListener('dragend', function () {
                              _this.destination = _this.destinationMarker.position;

                              _this.directionDraw();

                              _this.destinationDrag.emit(_this.destination);
                            });
                          }
                        } // Waypoints Marker


                        if (typeof _this.markerOptions.waypoints !== 'undefined') {
                          _this.waypoints.forEach(function (waypoint, index) {
                            // If waypoints are not array then set all the same
                            if (!Array.isArray(_this.markerOptions.waypoints)) {
                              _this.markerOptions.waypoints.map = map;
                              _this.markerOptions.waypoints.position = _route.via_waypoints[index];

                              _this.waypointsMarker.push(_this.setMarker(map, waypoint, _this.markerOptions.waypoints, _route.via_waypoints[index]));
                            } else {
                              _this.markerOptions.waypoints[index].map = map;
                              _this.markerOptions.waypoints[index].position = _route.via_waypoints[index];

                              _this.waypointsMarker.push(_this.setMarker(map, waypoint, _this.markerOptions.waypoints[index], _route.via_waypoints[index]));
                            }
                          }); // End forEach

                        }
                      } catch (err) {
                        console.error('MarkerOptions error.', err);
                      }
                    }

                    break;

                  default:
                    // console.warn(status);
                    break;
                } // End switch

              });
            }
          });
        }
        /**
         * Custom Origin and Destination Icon
         * \@memberof AgmDirection
         * @param {?} map map
         * @param {?} marker marker
         * @param {?} markerOpts properties
         * @param {?} content marker's infowindow content
         * @return {?} new marker
         */

      }, {
        key: "setMarker",
        value: function setMarker(map, marker, markerOpts, content) {
          var _this2 = this;

          if (typeof this.infoWindow === 'undefined') {
            this.infoWindow = new google.maps.InfoWindow({});
            this.sendInfoWindow.emit(this.infoWindow);
          }

          marker = new google.maps.Marker(markerOpts); // https://developers.google.com/maps/documentation/javascript/reference/marker?hl=zh-tw#MarkerOptions.clickable

          if (marker.clickable) {
            marker.addListener('click', function () {
              /** @type {?} */
              var infowindoContent = typeof markerOpts.infoWindow === 'undefined' ? content : markerOpts.infoWindow;

              _this2.infoWindow.setContent(infowindoContent);

              _this2.infoWindow.open(map, marker);
            });
          }

          return marker;
        }
        /**
         * This event is fired when remove markers
         * @return {?}
         */

      }, {
        key: "removeMarkers",
        value: function removeMarkers() {
          if (typeof this.originMarker !== 'undefined') {
            this.originMarker.setMap(null);
          }

          if (typeof this.destinationMarker !== 'undefined') {
            this.destinationMarker.setMap(null);
          }

          this.waypointsMarker.forEach(function (w) {
            if (typeof w !== 'undefined') {
              w.setMap(null);
            }
          });
        }
        /**
         * This event is fired when remove directions
         * @return {?}
         */

      }, {
        key: "removeDirections",
        value: function removeDirections() {
          if (this.directionsDisplay !== undefined) {
            this.directionsDisplay.setPanel(null);
            this.directionsDisplay.setMap(null);
            this.directionsDisplay = undefined;
          }
        }
        /**
         * This event is fired when destroy markers
         * @return {?}
         */

      }, {
        key: "destroyMarkers",
        value: function destroyMarkers() {
          // Remove origin markers
          try {
            if (typeof this.originMarker !== 'undefined') {
              google.maps.event.clearListeners(this.originMarker, 'click');

              if (this.markerOptions.origin.draggable) {
                google.maps.event.clearListeners(this.originMarker, 'dragend');
              }
            }

            if (typeof this.destinationMarker !== 'undefined') {
              google.maps.event.clearListeners(this.destinationMarker, 'click');

              if (this.markerOptions.origin.draggable) {
                google.maps.event.clearListeners(this.destinationMarker, 'dragend');
              }
            }

            this.waypointsMarker.forEach(function (w) {
              if (typeof w !== 'undefined') {
                google.maps.event.clearListeners(w, 'click');
              }
            });
            this.removeMarkers();
          } catch (err) {
            console.error('Can not reset custom marker.', err);
          }
        }
      }]);

      return AgmDirection;
    }();

    AgmDirection.ɵfac = function AgmDirection_Factory(t) {
      return new (t || AgmDirection)(_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdirectiveInject"](_agm_core__WEBPACK_IMPORTED_MODULE_1__["GoogleMapsAPIWrapper"]));
    };

    AgmDirection.ɵdir = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineDirective"]({
      type: AgmDirection,
      selectors: [["agm-direction"]],
      inputs: {
        travelMode: "travelMode",
        transitOptions: "transitOptions",
        drivingOptions: "drivingOptions",
        waypoints: "waypoints",
        optimizeWaypoints: "optimizeWaypoints",
        provideRouteAlternatives: "provideRouteAlternatives",
        avoidHighways: "avoidHighways",
        avoidTolls: "avoidTolls",
        visible: "visible",
        renderRoute: "renderRoute",
        origin: "origin",
        destination: "destination",
        infoWindow: "infoWindow",
        renderOptions: "renderOptions",
        panel: "panel",
        markerOptions: "markerOptions"
      },
      outputs: {
        onChange: "onChange",
        onResponse: "onResponse",
        sendInfoWindow: "sendInfoWindow",
        status: "status",
        originDrag: "originDrag",
        destinationDrag: "destinationDrag"
      },
      features: [_angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵNgOnChangesFeature"]]
    });
    /** @nocollapse */

    AgmDirection.ctorParameters = function () {
      return [{
        type: _agm_core__WEBPACK_IMPORTED_MODULE_1__["GoogleMapsAPIWrapper"]
      }];
    };

    AgmDirection.propDecorators = {
      origin: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      destination: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      travelMode: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      transitOptions: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      drivingOptions: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      waypoints: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      optimizeWaypoints: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      provideRouteAlternatives: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      avoidHighways: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      avoidTolls: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      renderOptions: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      panel: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      markerOptions: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      infoWindow: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      visible: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      renderRoute: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
      }],
      onChange: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      onResponse: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      sendInfoWindow: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      status: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      originDrag: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }],
      destinationDrag: [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
      }]
    };
    /*@__PURE__*/

    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AgmDirection, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Directive"],
        args: [{
          selector: 'agm-direction'
        }]
      }], function () {
        return [{
          type: _agm_core__WEBPACK_IMPORTED_MODULE_1__["GoogleMapsAPIWrapper"]
        }];
      }, {
        travelMode: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        transitOptions: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        drivingOptions: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        waypoints: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        optimizeWaypoints: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        provideRouteAlternatives: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        avoidHighways: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        avoidTolls: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        visible: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        onChange: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        onResponse: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        sendInfoWindow: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        status: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        originDrag: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        destinationDrag: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Output"]
        }],
        renderRoute: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        origin: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        destination: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        infoWindow: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        renderOptions: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        panel: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }],
        markerOptions: [{
          type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["Input"]
        }]
      });
    })();
    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,uselessCode} checked by tsc
     */


    var AgmDirectionModule = /*#__PURE__*/function () {
      function AgmDirectionModule() {
        _classCallCheck(this, AgmDirectionModule);
      }

      _createClass(AgmDirectionModule, null, [{
        key: "forRoot",
        value:
        /**
         * @return {?}
         */
        function forRoot() {
          return {
            ngModule: AgmDirectionModule
          };
        }
      }]);

      return AgmDirectionModule;
    }();

    AgmDirectionModule.ɵmod = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineNgModule"]({
      type: AgmDirectionModule
    });
    AgmDirectionModule.ɵinj = _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵdefineInjector"]({
      factory: function AgmDirectionModule_Factory(t) {
        return new (t || AgmDirectionModule)();
      },
      imports: [[]]
    });

    (function () {
      (typeof ngJitMode === "undefined" || ngJitMode) && _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵɵsetNgModuleScope"](AgmDirectionModule, {
        declarations: [AgmDirection],
        exports: [AgmDirection]
      });
    })();
    /*@__PURE__*/


    (function () {
      _angular_core__WEBPACK_IMPORTED_MODULE_0__["ɵsetClassMetadata"](AgmDirectionModule, [{
        type: _angular_core__WEBPACK_IMPORTED_MODULE_0__["NgModule"],
        args: [{
          imports: [],
          declarations: [AgmDirection],
          exports: [AgmDirection]
        }]
      }], null, null);
    })();
    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,uselessCode} checked by tsc
     */

    /**
     * @fileoverview added by tsickle
     * @suppress {checkTypes,extraRequire,uselessCode} checked by tsc
     */
    //# sourceMappingURL=agm-direction.js.map

    /***/

  }
}]);
//# sourceMappingURL=default~buyer-dashboard-buyer-dashboard-module~buyer-wally-board-buyer-wally-board-module~carrier-ca~ed64cf5c-es5.js.map