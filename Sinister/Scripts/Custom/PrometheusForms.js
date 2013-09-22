function SetField(FormId, Name, Value) {
    if ($('Form' + FormId + ' input#' + Name).attr("name") != undefined) {
        $('Form' + FormId + ' input#' + Name).attr("value", Value);
    }
    else {
        $('Form' + FormId).append('<input type="hidden" id="' + Name + '" name="' + Name + '" value="' + Value + '" />');
    }
}
function getUrlVars(url) {
    var vars = [], hash;
    var hashes = url.slice(url.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

var backup;
var cancel;
var refreshId;

function BindEvents() {
    //(".datepicker").remove();
    $('[data-date="True"]').each(function (e) {
        $(this).datepicker();
    });
    $('.validation-summary-errors').addClass('alert alert-danger');
    $('.validation-summary-errors').removeClass('validation-summary-errors');

    $('input[type="text"]').addClass("form-control");

    $.ajaxSetup({
        // Disable caching of AJAX responses
        cache: false
    });

    $(".modal").on("show.bs.modal", function () {
        //id = $(this).attr("id");
        backup = $(this).find('div.modal-body').html();
        cancel = false;
    });
    $(".modal").on("hidden.bs.modal", function () {
        $(this).find('div.modal-body').html(backup);
        if (cancel) {
            $("#" + refreshId).find("a[data-click='true'][data-toggle='remove']").click();
            $("#" + refreshId).find("a[data-click='true'][data-toggle='remove']").attr("data-click", "");
        }
    });
    //Модальные окна
    $('a[data-toggle="prometheusmodal"]').unbind("click");
    $('a[data-toggle="prometheusmodal"]').click(function (e) {
        modalid = $(this).attr("data-target");
        selectid = $(this).attr("data-select");
        if (selectid == undefined) selectid = "";
        refreshid = $(this).attr("data-refresh");
        if (refreshid == undefined) refreshid = "";

        href = $(this).attr("href");
        $.ajax({
            dataType: 'html',
            url: href,
            success: function (data) {
                id = $(data).find('div[data-modal="Wrapper"]').attr("id");
               
                $(modalid).html($(data).find('div[data-modal="Wrapper"]').wrap('<div>').parent().html());
                if (selectid != "")
                    $("#" + id).attr("data-select", selectid);
                if (refreshid != "")
                    $("#" + id).attr("data-refresh", refreshid);
                DecorateModal(id, modalid);
                BindEvents();

                var index_highest = 0;
                $('div.modal').each(function () {
                    var index_current = parseInt($(this).css("z-index"), 10);
                    if (index_current > index_highest) {
                        index_highest = index_current;
                    }
                });
                $(modalid).css("z-index", index_highest + 1);
                // $(".modal-backdrop").css("z-index", index_highest);
                $(modalid).modal();

                $(modalid).unbind('hide');;
                $(modalid).on('hidden', function (e) {
                   $(this).html("---")
                });
            }

        });
       e.preventDefault();
  });

    $('input[type="submit"],button[type="submit"]').unbind("click");
    $('input[type="submit"],button[type="submit"]').bind("click", function(e) { submitHandler(e, this); });
    $('a[role="button"]').unbind("click");
    $('a[role="button"]').bind("click", function(e) { submitHandler(e, this); });

    function submitHandler(e, that) {
        //ModalTarget = $(this).attr("data-target");
        //modalID = $(this).closest('div[data-modal="Wrapper"]').attr("id");

        var formId = $(that).closest("Form").attr("id");
        if (formId == undefined) formId = "";
        else formId = "#" + formId;

        var href = $(that).attr("href");
        if (href != undefined) {
            var params = getUrlVars(href);
            jQuery.each(params, function(i, val) {
                SetField(formId, val, params[val]);
            });
            href = href.split('?')[0];
            $("Form" + formId).attr("action", href);
        } else {
            var subAction = $(that).attr("data-action");
            if (subAction != undefined) SetField(formId, 'SubAction', subAction);
        }
        refreshId = "";
        refreshId = $(that).attr("data-refresh");
        if (refreshId == undefined) refreshId = "";
        if (refreshId != "") {
            e.preventDefault();
            $.ajax({
                dataType: "html",
                type: $('Form' + formId).attr('Method'),
                url: $('Form' + formId).attr('Action'),
                data: $('Form' + formId).serialize(),
                success: function(data) {

                    //if ($(data).find('#needlogin').html() != null) {
                    //    return;
                    //}
                    //if ($(data).find('#error').html() != null) {
                    //    return;
                    //}

                    $('#' + refreshId).html($(data).find('#' + refreshId).html());
                    cancel = true;
                     backup = $(data).find('#' + refreshId + '_body').html();
                    $(that).closest(".modal").modal('hide');
                    
                   
                    $('#' + refreshId).find("a[data-click='true'][data-toggle='modal']").click();
                    $('#' + refreshId).find("a[data-click='true'][data-toggle='modal']").attr("data-click", "");
                    //if ($(data).find('#' + refreshId).html() == undefined)
                    //if (ModalTarget != undefined) {
                    //   supresswizard=$(ModalTarget).attr("data-supresswizard");
                    //    //Если выходим из модального окна с подтверждением и попали назад в список -- закрыть окно
                    //    newstep = $(data).find('#wizardsteps').html();
                    //    oldstep = $('#wizardsteps').html();
                    //    if (newstep != oldstep) {
                    //        //Если мы в мастере, и поменялся шаг -- надо перейти на новый шаг.
                    //        //Для этого тупо переходим по ссылке
                    //       if (supresswizard!="true") window.location = $(data).find('.container').attr("data-url");
                    //    }
                    //    $(ModalTarget).modal('hide');
                    //    return;
                    //}
                    BindEvents();
                    // if (SubAction == undefined ||SubAction=="")
                    // DecorateModal(modalID, ModalTarget);
                }
            });
        }
    }

//        ModalTarget = $(this).attr("data-target");
//        modalID = $(this).closest('div[data-modal="Wrapper"]').attr("id");

//        FormId = $(this).closest("Form").attr("id");
//        if (FormId == undefined) FormId = ""; else FormId = "#" + FormId;
//        e.preventDefault();
//        href = $(this).attr("href");
//        params = getUrlVars(href);
//        jQuery.each(params, function (i, val) {
//            SetField(FormId, val, params[val]);
//        });
//        href = href.split('?')[0];
//        $("Form" + FormId).attr("action", href);
//        refreshId = $(this).attr("data-refresh");
//        if (refreshId == undefined) refreshId = "";
//        if (refreshId != "") {

//            $.ajax({
//                type: $('Form' + FormId).attr('Method'),
//                url: $('Form' + FormId).attr('Action'),
//                data: $('Form' + FormId).serialize(),
//                success: function (data) {
//                    if ($(data).find('#needlogin').html() != null) {
//                        return;
//                    }
//                    if ($(data).find('#error').html() != null) {
//                        return;
//                    }
//                    $('#' + refreshId).html($(data).find('#' + refreshId).html());
                   
//                    BindEvents();

//                    if (SubAction == undefined)
//                    DecorateModal(modalID, ModalTarget);

//                }
//            });
//        }
//        else {
//            $("Form" + FormId).submit();
//        }
//    });

}

function DecorateModal(id, modalid) {

    if ($("div#" + id).parent(".modal").attr("class") == undefined) return;

    selectid_ = $("#" + id).attr("data-select");
    refreshID_ = $("#" + id).attr("data-refresh");

    if (selectid_ == undefined) selectid_ = "";
    if (selectid_ != "") {
        $("#" + id).find('a[data-select]').attr("data-select", selectid_);
        $("#" + id).find('a[data-select="' + selectid_ + '"]').each(function (e) {
            $(this).attr("data-refresh", refreshID_);
            $(this).unbind("click");
            $(this).bind("click", function (e) {
                e.preventDefault();
                objid = $(this).attr("data-value");
                $(this).parents(".modal").modal('hide');
                fieldid = $(this).attr("data-select");
                $("#" + fieldid).attr("value", objid);
                
                action=$("#" + fieldid).attr("data-refresh-action");
                if (action == undefined) action = "";
                if (action == "") return;
                FormId = $("#" + fieldid).closest("Form").attr("id");
                refreshID = $("#" + fieldid).attr("data-val-refresh-id");
                if (refreshID == undefined || refreshID == "") refreshID = ""; else refreshID = "#" + refreshID;
                if (refreshID == "") return;
                    if (FormId == undefined) FormId = ""; else FormId = "#" + FormId;
                    $.ajax({
                        type: $('Form' + FormId).attr('Method'),
                        url: action,
                        data: $('Form' + FormId).serialize(),
                        success: function (data) {
                            if ($(data).find('#needlogin').html() != null) {
                                return;
                            }
                            if ($(data).find('#error').html() != null) {
                                return;
                            }
                            focused = $(':focus');
                            src = $(data).find('div' + refreshID);
                            //alert(src.html());
                            dst = $('div' + refreshID);
                            dst.html(src.html());
                            setTimeout(function () { $('#' + focused.attr("id")).focus(); }, 10);
                            BindEvents();
                            SetValidation(action);
                        }
                });
            });
        });
    }

    $fieldset = $("div#" + id).parent();
    if ($fieldset != undefined) {
        $("div#" + id).parent().find("form").css("margin", "0");
        $fieldset.find('div.form-actions').removeClass('form-actions').toggleClass('modal-footer');
        legend = $fieldset.find('legend').html();
        //legend = '<h3>'+legend+'</h3>';
        $fieldset.find('legend').contents().unwrap().wrap('<h3>').parent().wrap('<div>').parent().toggleClass('modal-header').insertBefore($fieldset.find('fieldset'));
       


        if (legend != undefined) {
            if ($fieldset.find('fieldset').parent("div").attr("class") == undefined)
                $fieldset.find('fieldset').wrap('<div>').parent().toggleClass('modal-body');
            $fieldset.find('fieldset').before($(legend).html());
        } else {
            if ($("div#" + id).parents(".modal").attr("id") != undefined) {
                $("div#" + id).attr("class", 'modal-body');
            }
        }

      //  if ($fieldset.find('fieldset').parent("div").attr("class") == undefined)
       //     $fieldset.find('fieldset').wrap('<div>').parent().toggleClass('modal-header');


        $(modalid + '.wider').find('.modal-body').css(
            "max-height", $(window).height() - 350);



        $fieldset.find('input[type="submit"]:not([data-refresh])').attr("data-refresh", id);
        $fieldset.find('input[type="submit"]').attr("data-target", modalid);
        $fieldset.find('input[type="submit"]').attr("data-confirm", "modal");




    }
}

function SetRadio(id) {
    id = id.replace(/\[/g, "_")
        .replace(/\]/g, "_")
        .replace(/\./g, "_");
    value = $('#' + id).attr('value');
    css = $('#' + id).attr('class');
    if (css == "input-validation-error")
        css = "btn-danger";
    else
        css = $('#' + id).parent().children('div[data-toggle="buttons-radio"]').attr("data-toggle-css");
    if (css == undefined) css = "btn-primary";
    $('#' + id).parent().children('div[data-toggle="buttons-radio"]').attr("data-toggle-css", css);
    $('#' + id).parent().children('div[data-toggle="buttons-radio"]').children('button').removeClass('active ' + css);
    $('#' + id).parent().children('div[data-toggle="buttons-radio"]').children('button[value=' + value + ']').toggleClass('active ' + css);
    SetImages(id);
    $('#' + id).parent().children('div[data-toggle="buttons-radio"]').children('button').click(function () {
        $('#' + id).attr('value', $(this).attr('value'));
        $('#' + id).change();
        value = $('#' + id).attr('value');
        SetImages(id);
    });
}


function SetImages(id) {
    $('#' + id).parent().children('div[data-toggle="buttons-radio"]').children('button').children('i').removeClass('icon-white');
    $('#' + id).parent().children('div[data-toggle="buttons-radio"]').children('button[value=' + value + ']').children('i').toggleClass('icon-white');
}


function SetCombo(id) {
    id = id.replace(/\[/g, "_")
        .replace(/\]/g, "_")
        .replace(/\./g, "_");
    css = $('#' + id).attr('class');


    if (css == "input-validation-error")
        $("#" + id + "_Combo").toggleClass(css);
    
    $("#" + id + "_Combo [value='" + $("#" + id).attr("Value") + "']").prop('selected', true);

    $("#" + id + "_Combo").change(function (e) {
        $("#" + id).attr("Value", $(this).find("option:selected").val());
        $('#' + id).change();

    });

}