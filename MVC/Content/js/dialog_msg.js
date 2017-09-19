//初始化加载dialog
$(function() {
	$("#dialog").dialog({
		bgiframe: true,
		resizable: false,
		height:150,
		width:280,
		autoOpen: false,
		modal: true,
		overlay: {
			backgroundColor: '#000',
			opacity: 0.5
		}
	});
});

//打开dialog,隐藏X
function openDialog(){
	$('a.ui-dialog-titlebar-close').hide(); 
	$("#dialog").dialog('open');
}

//关闭dialog,隐藏X
function closeDialog(){
	$('a.ui-dialog-titlebar-close').hide(); 
	$("#dialog").dialog('close');
}