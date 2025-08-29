$(function () {

    GetIncidentList();

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

async function SaveIncidentForm() {
    try {
        let formData = new FormData();
        let obj = $("#NewIncidentForm")[0];

        // Add files
        $(obj).find("input[type='file']").each(function (i, tag) {
            for (let file of tag.files) {
                formData.append(tag.name, file);
            }
        });

        // Serialize other fields
        let params = $(obj).serializeArray();
        let assetIds = [];

        $.each(params, function (i, val) {
            if (val.name === "asset.Id") {
                assetIds.push(val.value);
            } else {
                formData.append(val.name, val.value);
            }
        });

        // Add AssetIds
        if (assetIds.length > 0) {
            formData.append("incidentiLocation.AssetIDs", assetIds.join(","));
        }

        showLoader($("#addIncidentModal"));

        // Send request
        let response = await fetch("/Incidents/SaveIncident", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            SwalSuccessAlert(result.data);
            $(".btn-close").trigger("click");
            GetIncidentList();
        } else {
            SwalErrorAlert(result.message || "Failed to save incident.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving incident!");
        console.error(error);
    } finally {
        hideLoader($("#addIncidentModal"));
    }
}

async function GetIncidentList() {
    try {

        showLoader($(".main-content"));

        const response = await fetch("/Incidents/GetIncidentList", {
            method: "GET",
            headers: { "Accept": "text/html" }
        });

        if (!response.ok) throw new Error("Failed to load cart layout");

        const content = await response.text();

        $("#incidentGrid").empty().html(content);

    } catch (error) {
        console.error("Error updating shopping cart:", error);
    }
    finally {
        hideLoader($(".main-content"));
    }
}