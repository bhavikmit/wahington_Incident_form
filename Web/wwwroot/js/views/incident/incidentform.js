$(function () {

    $(document).off("click", "#nextToIncidentLocation");
    $(document).on("click", "#nextToIncidentLocation", function (e) {
        e.preventDefault();

        var isValid = true;

        // Loop through all required fields
        $("#pills-caller").find("input[data-val-required], select[data-val-required], textarea[data-val-required], #relationaship, #CallTimedatetime").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            // Dropdown special check
            if ($field.is("select") && (value === "" || value === "--Select--")) {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else if (value === "") {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else {
                clearError($field);
            }
        });

        if (isValid) {
            $("#pills-location-tab").trigger("click");
        }
    });

    $(document).off("click", "#nextToIncidentDetials");
    $(document).on("click", "#nextToIncidentDetials", function (e) {
        e.preventDefault();

        var isValid = true;

        // Loop through all required fields
        $("#pills-location").find("input[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            // Dropdown special check
            if ($field.is("select") && (value === "" || value === "--Select--")) {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else if (value === "") {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else {
                clearError($field);
            }
        });

        if (isValid) {
            $("#pills-detail-tab").trigger("click");
        }
    });

    $(document).off("click", "#nextToDescriptionIssue");
    $(document).on("click", "#nextToDescriptionIssue", function (e) {
        e.preventDefault();

        var isValid = true;

        // Loop through all required fields
        $("#pills-detail").find("#eventTypeId").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            // Dropdown special check
            if ($field.is("select") && (value === "" || value === "--Select--")) {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else if (value === "") {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else {
                clearError($field);
            }
        });

        if (isValid) {
            $("#pills-description-tab").trigger("click");
        }
    });

    $(document).off("click", "#nextToSeverity");
    $(document).on("click", "#nextToSeverity", function (e) {
        e.preventDefault();

        var isValid = true;

        // Loop through all required fields
        $("#pills-description").find("textarea[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            // Dropdown special check
            if ($field.is("select") && (value === "" || value === "--Select--")) {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else if (value === "") {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else {
                clearError($field);
            }
        });

        if (isValid) {
            $("#pills-severity-tab").trigger("click");
        }
    });

    $(document).off("click", "#nextToEnvironmental");
    $(document).on("click", "#nextToEnvironmental", function (e) {
        e.preventDefault();

        var isValid = true;

        // Loop through all required fields
        $("#pills-severity").find("#severity").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            // Dropdown special check
            if ($field.is("select") && (value === "" || value === "--Select--")) {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else if (value === "") {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else {
                clearError($field);
            }
        });

        if (isValid) {
            $("#pills-safety-tab").trigger("click");
        }
    });

    $(document).off("click", "#nextToSupportInfo");
    $(document).on("click", "#nextToSupportInfo", function (e) {
        e.preventDefault();

        var isValid = true;

        // Loop through all required fields
        $("#pills-safety").find("input[data-val-required], select[data-val-required], textarea[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            // Dropdown special check
            if ($field.is("select") && (value === "" || value === "--Select--")) {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else if (value === "") {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else {
                clearError($field);
            }
        });

        if (isValid) {
            $("#pills-support-tab").trigger("click");
        }
    });

    $(document).on("click", "#btn_Incident_Save", function (e) {
        e.preventDefault();

        var isValid = true;

        // Loop through all required fields
        $("#pills-support").find("input[data-val-required], select[data-val-required], textarea[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            // Dropdown special check
            if ($field.is("select") && (value === "" || value === "--Select--")) {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else if (value === "") {
                isValid = false;
                showError($field, $field.attr("data-val-required"));
            }
            else {
                clearError($field);
            }
        });

        if (isValid) {
            SaveIncidentForm();
        }
    });

    // Show error: red border + message
    function showError($field, message) {
        $field.css("border", "1px solid red");

        //var $errorSpan = $field.siblings(".field-validation-error");

        //if ($errorSpan.length === 0) {
        //    $errorSpan = $("<span class='field-validation-error text-danger'></span>");
        //    $field.after($errorSpan);
        //}
        //$errorSpan.text(message);
    }

    // Clear error: reset border + remove message
    function clearError($field) {
        $field.css("border", ""); // reset to default
        $field.siblings(".field-validation-error").remove();
    }
});

function ShowImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#image-thumbnail').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}
function RemoveImage() {
    document.getElementById("Image").value = "";
    $("#image-thumbnail").removeAttr('src');
    $("#hidden-image-url").val(null);
}

function SaveIncidentForm() {
    debugger

    let form = [];
    var formData = new FormData();
    var obj = $("#NewIncidentForm");

    // Add files
    $.each($(obj).find("input[type='file']"), function (i, tag) {
        $.each($(tag)[0].files, function (i, file) {
            formData.append(tag.name, file);
            form.push({ name: tag.name, file: file });
        });
    });

    var params = $(obj).serializeArray();
    var assetIds = [];

    $.each(params, function (i, val) {
        if (val.name === "asset.Id") {
            assetIds.push(val.value); // collect all asset IDs
        } else {
            formData.append(val.name, val.value);
            form.push({ name: val.name, value: val.value });
        }
    });




    // Add AssetIds as combined value
    if (assetIds.length > 0) {
        // option 1: as comma-separated string
        formData.append("AssetIDs", assetIds.join(","));

        // option 2: as JSON array (better if API accepts it)
        // formData.append("AssetIds", JSON.stringify(assetIds));

        form.push({ name: "AssetIDs", value: assetIds.join(",") });
    }

    console.log(form);
    //return formData;


    //var formData = $("#NewIncidentForm").serializeFiles(); // change #incidentForm to your form ID

    $.ajax({
        url: "/Incidents/SaveIncident",   // 🔹 change to your API/Controller URL
        type: "POST",
        data: formData,
        processData: false, // prevent jQuery from processing
        contentType: false, // prevent jQuery from setting content type
        success: function (response) {
            console.log("Form saved successfully:", response);
            alert("Incident saved successfully!");
        },
        error: function (xhr, status, error) {
            console.error("Error saving incident:", error);
            alert("Error while saving incident!");
        }
    });
}