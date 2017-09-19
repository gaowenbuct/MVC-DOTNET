(function($) {
	$.fn.extend( {
		// 弹出选择框，选定后返回到主叫页面
			searchDialog : function(options) {
				var defaults = {
					searchDialogId : 'searchDialog',
					title : '查询',
					url : '',
					target : '',
					targetId : '',
					clickCallback : null,
					autoOpen : false,
					modal : true,
					resizable : true,
					autoResize : true,
					width : 640,
					height : 460,
					okButtonCallback : null
				};
				
				var options = $.extend(defaults, options);
				var o = options;
				var frameId = 'if_' + o.searchDialogId;
					$(
						'<div id="'+o.searchDialogId+'"><iframe id="'
								+ frameId + '" src="'
								+ o.url
								+ '" style="width: 100%; height: 100%;"  frameborder="0"></iframe></div>')
						.dialog( {
							autoOpen : o.autoOpen,
							modal : o.modal,
							resizable : o.resizable,
							autoResize : o.autoResize,
							width : o.width,
							height : o.height,
							title : o.title,
							buttons : {
								'取消' : function() {
									$('#'+frameId).attr("src", "");
									$(this).dialog("close");
								},
								'确定' : function() {
									var v = $("#"+frameId).contents().find('table input:radio:checked').val();
									if (!v) {
										$('input[name=' + o.target + ']').val('');
										$('input[name=' + o.targetId + ']').val('');
									}
									if (o.okButtonCallback) {
										o.okButtonCallback(frameId);
									} else if (v) {
										$('input[name=' + o.target + ']').val(v);
									}
									$('#'+frameId).attr("src", "");
									$(this).dialog("close");
								}
							}
						});
					return $(this).click(function(e) {
						e.preventDefault();
						if (o.clickCallback) {
							o.clickCallback(frameId);
						}
						$('#'+o.searchDialogId).dialog('open');
					});
			}
		});
})(jQuery);