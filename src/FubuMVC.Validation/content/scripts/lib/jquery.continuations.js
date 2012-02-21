// jquery.continuations v0.3.8
//
// Copyright (C)2011 Joshua Arnold, Jeremy Miller
// Distributed Under Apache License, Version 2.0
//
// https://github.com/DarthFubuMVC/jquery-continuations

(function ($, aggregator) {

    "use strict";

    // Sanity check of dependencies
    if (typeof ($) !== 'function') {
        throw 'jQuery.continuations: jQuery not found.';
    }

    var CORRELATION_ID = 'X-Correlation-Id';
    var policies = [];
	
	var theContinuation = function () { };
    theContinuation.prototype = {
        success: false,
        errors: [],
        refresh: false,
        correlationId: null,
		options: {},
		matchOnProperty: function(prop, predicate) {
			return typeof(this[prop]) !== 'undefined' && predicate(this[prop]);
		},
        isCorrelated: function () {
			return this.matchOnProperty('correlationId', function(id) {
				return id != null;
			});
        }
    };

    var refreshPolicy = function () {
        this.matches = function (continuation) {
            return continuation.refresh && continuation.refresh.toString() === 'true';
        };
        this.execute = function (continuation) {
            $.continuations.windowService.refresh();
        };
    };

    var navigatePolicy = function () {
        this.matches = function (continuation) {
            return continuation.navigatePage != undefined && continuation.navigatePage != '';
        };
        this.execute = function (continuation) {
            $.continuations.windowService.navigateTo(continuation.navigatePage);
        };
    };

    var errorPolicy = function () {
        this.matches = function (continuation) {
            return continuation.errors && continuation.errors.length != 0;
        };
        this.execute = function (continuation) {
            $.continuations.eventAggregator.publish('ContinuationError', continuation);
        };
    };

    var payloadPolicy = function () {
        this.matches = function (continuation) {
            return continuation.topic != null && continuation.payload != null;
        };
        this.execute = function (continuation) {
            $.continuations.eventAggregator.publish(continuation.topic, continuation.payload);
        };
    };

    var continuations = function () { };
    continuations.prototype = {
        init: function () {
            $(document).ajaxComplete(function (e, xhr, options) {
                $.continuations.eventAggregator.publish('AjaxCompleted', {
                    correlationId: xhr.getResponseHeader(CORRELATION_ID)
                });
            });

            var self = this;
            $.ajaxSetup({
                cache: false,
                success: function (continuation, status, jqXHR) {
					var options = this.options;
					if(typeof(options) === 'undefined') {
						options = {};
					}
					if(typeof(continuation) !== 'undefined') {
						continuation.options = options;
					}
					
                    self.onSuccess({
                        continuation: continuation,
						callback: this.continuationSuccess,
                        status: status,
                        response: jqXHR
                    });
                },
                beforeSend: function (xhr, settings) {
                    self.setupRequest(xhr, settings);
                }
            });

            this.setupDefaults();
        },
        setupDefaults: function () {
            this.applyPolicy(new refreshPolicy());
            this.applyPolicy(new navigatePolicy());
            this.applyPolicy(new errorPolicy());
            this.applyPolicy(new payloadPolicy());
        },
        onSuccess: function (msg) {
            var contentType = msg.response.getResponseHeader('Content-Type');
            if (!contentType || contentType.indexOf('json') == -1) {
                return;
            }

            var continuation = msg.continuation;
            continuation.correlationId = msg.response.getResponseHeader('X-Correlation-Id');
			
			if($.isFunction(msg.callback)) {
				msg.callback(continuation);
			}

            this.process(continuation);
        },
        // Keep this public for form correlation
        setupRequest: function (xhr, settings) {
            // this could come from the ajax options
            var id = settings.correlationId;
            if (typeof(id) === 'undefined') {
                id = new Date().getTime().toString();
            }
            xhr.setRequestHeader(CORRELATION_ID, id);
            $.continuations.eventAggregator.publish('AjaxStarted', {
                correlationId: id
            });
        },
        applyPolicy: function (policy) {
            policies.push(policy);
            return this;
        },
		// Mostly for testing
		reset: function() {
			policies.length = 0;
			this.setupDefaults();
		},
        process: function (continuation) {
			var standardContinuation = new $.continuations.continuation();
			continuation = $.extend(standardContinuation, continuation);
            var matchingPolicies = [];
            for (var i = 0; i < policies.length; ++i) {
                var p = policies[i];
                if (p.matches(continuation)) {
                    matchingPolicies.push(p);
                }
            }

            for (var i = 0; i < matchingPolicies.length; ++i) {
                matchingPolicies[i].execute(continuation);
            }
        },
        useAmplify: function () {
            $.continuations.eventAggregator = amplify;
        }
    };

    continuations.prototype.windowService = {
        refresh: function () {
            window.location.reload();
        },
        navigateTo: function (url) {
            window.location = url;
        }
    };

    continuations.prototype.eventAggregator = {
        publish: function (topic, payload) {
            // no-op
        },
		subscribe: function(topic, context, callback) {
			// no-op
		}
    };

    var module = new continuations();
    module.init();


    // Make it global
    $.continuations = module;
	$.continuations.useAmplify();
	$.continuations.continuation = theContinuation;
	
	$.fn.correlatedSubmit = function (options) {
		if(typeof(options) === 'undefined') {
			options = {};
		}
		
        return this.each(function () {
            var self = $(this);
            var correlationId = options.correlationId;
            if (typeof(correlationId) === 'undefined') {
                var id = self.attr('id');
                if (!id) {
                    id = 'form_' + new Date().getTime().toString();
                    self.attr('id', id);
                }

                correlationId = id;
            }

            self.ajaxSubmit({
				correlationId: correlationId,
				continuationSuccess: function(continuation) {
					continuation.form = self;
					continuation.options = options;
					
					if($.isFunction(options.continuationSuccess)) {
						options.continuationSuccess(continuation);
					}
				}
            });
        });
    };
} (jQuery));