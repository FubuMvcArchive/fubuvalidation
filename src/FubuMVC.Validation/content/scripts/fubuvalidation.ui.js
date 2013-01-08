(function ($) {
	var exports = {
		Strategies: {}
	};

	function defineCore(name, clazz) {
		exports[name] = clazz;
	}

	function defineStrategy(name, clazz) {
		exports.Strategies[name] = clazz;
	}

	function DefaultHandler() {
		this.strategies = [];
	}

	DefaultHandler.prototype = {
		registerStrategy: function (strategy) {
			this.strategies.push(strategy);
		},
		strategiesMatching: function (continuation, action) {
			_.each(this.strategies, function (strategy) {
				if (strategy.matches(continuation)) {
					action(strategy);
				}
			});
		},
		process: function (continuation) {
			this.reset(continuation);
			this.strategiesMatching(continuation, function (strategy) {
				strategy.render(continuation);
			});
		},
		reset: function (continuation) {
			this.strategiesMatching(continuation, function (strategy) {
				strategy.reset(continuation);
			});
		}
	};

	DefaultHandler.basic = function () {
		var handler = new DefaultHandler();
		handler.registerStrategy(new ValidationSummaryStrategy());
		handler.registerStrategy(new ElementHighlightingStrategy());
		handler.registerStrategy(new InlineErrorStrategy());

		return handler;
	};

	function ValidationProcessor(handler) {
		this.handler = handler;
		this.finders = [];
	}

	ValidationProcessor.prototype = {
		useValidationHandler: function (handler) {
			this.handler = handler;
		},
		findElementsWith: function (finder) {
			this.finders.push(finder);
		},
		findElement: function (continuation, key, error) {
			var searchContext = {
				key: key,
				error: error,
				form: continuation.form
			};

			for (var i = 0; i < this.finders.length; i++) {
				var finder = this.finders[i];
				finder(searchContext);
			}

			return searchContext.element;
		},
		fillElements: function (continuation) {
			for (var i = 0; i < continuation.errors.length; i++) {
				var error = continuation.errors[i];
				if (!error.element && error.field) {
					error.element = this.findElement(continuation, error.field, error);
				}
			}
		},
		process: function (continuation) {
			this.fillElements(continuation);
			this.handler.process(continuation);
		},
		registerStrategy: function(strategy) {
            if (typeof(this.handler.registerStrategy) == 'function') {
                this.handler.registerStrategy(strategy);
            }
		},
		reset: function (continuation) {
			this.handler.reset(continuation);
		}
	};

	ValidationProcessor.basic = function() {
		var handler = DefaultHandler.basic();
		var processor = new ValidationProcessor(handler);

		return processor;
	};

	function tokenFor(error) {
		return error.label + ' - ' + error.message;
	}

	function ValidationSummaryStrategy() {
	}

	ValidationSummaryStrategy.prototype = {
		matches: function (continuation) {
			var value = continuation.form.data('validationSummary');
			return typeof (value) != 'undefined';
		},
		summaryContext: function (continuation) {
			var container = $('.validation-container', continuation.form);
			return {
				container: container,
				summary: container.find('ul.validation-summary')
			};
		},
		reset: function (continuation, context) {
            if(!context) {
            	context = this.summaryContext(continuation);
            }
			
            context.summary.html('');
            context.container.hide();
        },
		render: function (continuation) {
			var context = this.summaryContext(continuation);
			if (continuation.errors.length == 0) {
				return;
			}

			context.container.show();

			for(var i = 0; i < continuation.errors.length; i++) {
				var error = continuation.errors[i];
				this.append(context, error);
			}
		},
		append: function (context, error) {
			var found = false;
			context
				.summary
                .find("li[data-field='" + error.field + "']")
                .each(function () {
                	if (found) return;
                	if ($(this).find('a').html() == error.message) {
                		found = true;
                	}
                });

			if (!found) {
				var token = tokenFor(error);
				var item = $('<li data-field="' + error.field + '"><a href="javascript:void(0);">' + token + '</a></li>');
				
				if(error.element) {
					item.find('a').click(function() {
						item.element.focus();
					});
				}
				
				context.summary.append(item);
			}
		}
	};
	
	function ElementHighlightingStrategy() {		
	}

	ElementHighlightingStrategy.prototype = {
		matches: function (continuation) {
			var value = continuation.form.data('validationHighlight');
			return typeof (value) != 'undefined';
		},
		reset: function (continuation) {
			continuation.form.find('.error').each(function() {
				$(this).removeClass('error');
			});
		},
		render: function (continuation) {
			this.eachError(continuation, function(error) {
				error.element.addClass('error');
			});
		},
		eachError: function (continuation, action) {
			for(var i = 0; i < continuation.errors.length; i++) {
				var error = continuation.errors[i];
				if(error.element) {
					action(error);
				}
			}
		}
	};
	
	function InlineErrorStrategy() {
	}

	InlineErrorStrategy.prototype = {
		matches: function (continuation) {
			var value = continuation.form.data('validationInline');
			return typeof (value) != 'undefined';
		},
		reset: function (continuation) {
			continuation.form.find('.fubu-inline-error').each(function() {
				$(this).remove();
			});
		},
		render: function (continuation) {
			this.eachError(continuation, function(error) {
				var message = $('<span class="help-inline fubu-inline-error" data-field="' + error.field +  '">' + error.message + '</span>');

				error.element.after(message);
			});
		},
		eachError: function (continuation, action) {
			for(var i = 0; i < continuation.errors.length; i++) {
				var error = continuation.errors[i];
				if(error.element) {
					action(error);
				}
			}
		}
	};


	defineCore('DefaultHandler', DefaultHandler);
	defineCore('ValidationProcessor', ValidationProcessor);
	defineCore('TokenFor', tokenFor);

	defineStrategy('Summary', ValidationSummaryStrategy);
	defineStrategy('Highlighting', ElementHighlightingStrategy);
	defineStrategy('Inline', InlineErrorStrategy);

	$.extend(true, $, { 'fubuvalidation': { 'UI': exports} });
	
} (jQuery));