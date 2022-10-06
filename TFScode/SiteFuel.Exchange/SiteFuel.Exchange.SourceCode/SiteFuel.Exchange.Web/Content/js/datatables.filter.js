//This function initializes the content inside the filter modal
function configFilter($this, colArray) {
	setTimeout(function () {
		$('.' + $this[0].id).remove();
		var tableName = $this[0].id;
		var columns = $this.api().columns();
		$.each(colArray, function (i, arg) {
			var th = $('#' + tableName + ' th:eq(' + i + ')');
			var isFilterApplied = false;
			if (columns.state() != null) {
				isFilterApplied = columns.state().columns[arg].search.search != '' ? true : false;
			}
			if (!th.hasClass('sorting_disabled') && !th.hasClass('filtering-disabled'))
				th.append('<span class="fa fa-filter dt-filterIcon ' + (isFilterApplied ? "color-blue" : "") + '" onclick="showFilter(event, \'' + tableName + '_' + arg + '\',\'' + i + '\',\'' + tableName + '\')"></span>');
		});

		var template = '<div class="dt-modalFilter ' + $this[0].id + '">' +
			'<div class="dt-modal-content">{0}</div>' +
			'<div class="dt-modal-footer">' +
            '<a href="javascript:void(0)" onclick="clearFilter(this, {1}, {2}, \'{3}\');" id="Clear_{4}_' + $this[0].id + '_Filter" class="btn btn-danger btn-sm">Clear</a>' +
            '<a href="javascript:void(0)" onclick="performFilter(this, {1}, {2}, \'{3}\');" id="Apply_{4}_' + $this[0].id + '_Filter" class="btn btn-primary btn-sm pull-right">Ok</a>' +
			'</div>' +
			'</div>';
		$.each(colArray, function (index, value) {
			
			columns.every(function (i) {
				if (value === i && this.visible() == true) {
					var column = this;
					var columnName = $(this.header()).text().replace(/\s+/g, "_");
					var content = '<input type="text" id="Search_' + $this[0].id + '_' + columnName + 'FieldValue" class="form-control filterSearchText mb5" onkeyup="filterValues(this)" />';
					var distinctArray = [];
					var selectedValues = [];
					if (column.state() && column.state().columns[i].search.search != '') {
						selectedValues = column.state().columns[i].search.search.split('|');
					}
					column.data().each(function (d, j) {
                        if (/^<[a-z][\s\S]*>/i.test(d) == true) {
                            if ($(d).is('select'))
                                d = $(d).children("option").filter(":selected").text();
                            else
                                d = $(d).text();
                        }
                        else if (/<[a-z][\s\S]*>/i.test(d) == true && /\<.*.type=\"hidden".*\>/i.test(d) == true) {
                            d = d.replace(/\<.*.type=\"hidden".*\>/, '');
                        }

						var isSelected = false;
						$.each(selectedValues, function (ctr, searchText) {
							var regex = new RegExp('^' + searchText + '$');
							if (regex.test(d) == true) {
								isSelected = true;
								return;
							}
						});

						//if (distinctArray.indexOf(d) == -1) {
							//var id = tableName + "_" + columnName + "_" + j; // onchange="formatValues(this,' + value + ');
							//content += '<div><label class="checkbox checkbox-inline"><input type="checkbox" value="' + d + '"  id="' + id + '" ' + (isSelected ? "checked = \"checked\"" : "") + ' />' + d + '</label></div>';
							//distinctArray.push(d);
						//}
					});
					var newTemplate = $(template.replace('{0}', content).replace('{1}', value).replace('{1}', value).replace('{2}', index).replace('{2}', index).replace('{3}', tableName).replace('{3}', tableName).replace('{4}', columnName).replace('{4}', columnName));
					$('body').append(newTemplate);
					modalFilterArray[tableName + "_" + value] = newTemplate;
					content = '';
				}
			});
		});
	}, 50);
}
var modalFilterArray = {};
//User to show the filter modal
function showFilter(e, index,col_no,tname) {
	$('.dt-modalFilter').hide();
	var col_val = parseInt(col_no) + 1;
	var len = $('#' + tname).columnCount(); 
	if (col_val == len) {
		$(modalFilterArray[index]).css({ right: 0, top: 0 });
		var th = $(e.target).parent();
		var pos = th.offset();
		//$(modalFilterArray[index]).width(th.outerWidth() * 1);
		$(modalFilterArray[index]).css({ 'right': "60px", 'top': pos.top });
	} else {
		$(modalFilterArray[index]).css({ left: 0, top: 0 });
		var th = $(e.target).parent();
		var pos = th.offset();
		//$(modalFilterArray[index]).width(th.outerWidth() * 1);
		$(modalFilterArray[index]).css({ 'left': pos.left, 'top': pos.top });
	}
	$(modalFilterArray[index]).show();
	$('#dt-mask').show();
	e.stopPropagation();
}

$.fn.columnCount = function () {
	return $('th', $(this).find('thead')).length;
};

$.expr[":"].icontains = $.expr.createPseudo(function (arg) {
	return function (elem) {
		return $(elem).text().toUpperCase().indexOf(arg.toUpperCase()) >= 0;
	};
});

//This function is to use the searchbox to filter the checkbox
function filterValues(node) {
	var searchString = $(node).val().toUpperCase().trim();
    var rootNode = $(node).parent();
	if (searchString == '') {
		rootNode.find('div').show();
	} else {
		rootNode.find("div").hide();
		rootNode.find("div:icontains('" + searchString + "')").show();
	}
}

//Execute the filter on the table for a given column
function performFilter(node, i, colIndex, tableId) {
	var rootNode = $(node).parent().parent();
	var searchString = '', counter = 0;

    rootNode.find('input:text').each(function (index, textbox) {
        
        searchString = $(textbox).val();
    });
	rootNode.find('input:checkbox').each(function (index, checkbox) {
		if (checkbox.checked) {
            searchString += (searchString == '') ? checkbox.value : '|' + checkbox.value;
			counter++;
		}
	});
	searchString = searchString.replace(/\$/g, '\\$');
	searchString = searchString.replace(/\+/g, '\\+');
	searchString = searchString.replace(/\-/g, '\\-');
	searchString = searchString.replace(/\(/g, '\\(');
    searchString = searchString.replace(/\)/g, '\\)');
    searchString = searchString.trim();
	//if (searchString != '')
	//	searchString = '^' + searchString + '$';
    $('#' + tableId).DataTable().column(i).search(
        searchString, true, false, true
	).draw(false);
	if (searchString != '')
		$('#' + tableId + ' tr > th:eq(' + colIndex + ')').find('.dt-filterIcon').addClass('color-blue');
	else
		$('#' + tableId + ' tr > th:eq(' + colIndex + ')').find('.dt-filterIcon').removeClass('color-blue');
	rootNode.hide();
    $('#mask').hide();

	$('#' + tableId).DataTable().page(1).draw(true);  
}

//Removes the filter from the table for a given column
function clearFilter(node, i, colIndex, tableId) {
	var rootNode = $(node).parent().parent();
	rootNode.find(".filterSearchText").val('');
	rootNode.find('input:checkbox').each(function (index, checkbox) {
		checkbox.checked = false;
		$(checkbox).parent().show();
	});
	$('#' + tableId).DataTable().column(i).search(
		'',
		true, false
	).draw();
	$('#' + tableId + ' tr > th:eq(' + colIndex + ')').find('.dt-filterIcon').removeClass('color-blue');
	rootNode.hide();
	$('#mask').hide();
}

$(document).click(function (e) {
	var container = $(".dt-modalFilter:visible");
	if (!container.is(e.target) && container.has(e.target).length === 0) {
		container.hide();
	}
});