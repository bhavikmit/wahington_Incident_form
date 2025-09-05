$(function () {
    GetAllRelationships();

    // start setting tabs
    $(document).off("click", ".settingAllTab");
    $(document).on("click", ".settingAllTab", function (e) {
        e.preventDefault();
        var tab = $(this).attr("data-tab");
        if (tab === "source") {
            GetAllRelationships();
        }
        else if (tab === "event") {
            GetAllEventTypes();
        }
        else if (tab === "severity") {
            GetAllSeverity();
        }
        else if (tab === "status") {
            GetAllStatusLegend();
        }
        else if (tab === "asset") {
            GetAllAssetIds();
        }
        else if (tab === "type") {
            GetAllAssetTypes();
        }
        else if (tab === "teams") {
            GetAllIncidentTeams();
        }
    });
    // end setting tabs

    // Start Source
    $(document).off("click", ".btnAddNewSource");
    $(document).on("click", ".btnAddNewSource", function (e) {
        e.preventDefault();
        AddRelationships();
    });

    $(document).off("click", ".cancelSource");
    $(document).on("click", ".cancelSource", function (e) {
        e.preventDefault();
        $("#addSource").empty().html('');
        $('li.active').trigger('click')
    });

    $(document).off("click", ".saveSource");
    $(document).on("click", ".saveSource", function (e) {
        e.preventDefault();
        var isValid = true;

        $("#saveSourceDiv").find("input[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            if (value === "") {
                isValid = false;
                showError($field);
            }
            else {
                clearError($field);
            }
        });
        if (isValid) {
            SaveRelationships();
        }
    });

    $(document).off("click", ".editSource");
    $(document).on("click", ".editSource", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetRelationshipById(id);
    });

    $(document).off("click", ".deleteSource");
    $(document).on("click", ".deleteSource", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        DeleteSourceItem(id);
    });

    // End Source


    // Start Event Type
    $(document).off("click", ".btnAddNewEvent");
    $(document).on("click", ".btnAddNewEvent", function (e) {
        e.preventDefault();
        AddEventType();
    });

    $(document).off("click", ".cancelEventType");
    $(document).on("click", ".cancelEventType", function (e) {
        e.preventDefault();
        $("#addEventType").empty().html('');
        $('li.active').trigger('click')
    });

    $(document).off("click", ".saveEventType");
    $(document).on("click", ".saveEventType", function (e) {
        e.preventDefault();
        var isValid = true;

        $("#saveEventTypeDiv").find("input[data-val-required], textarea[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            if (value === "") {
                isValid = false;
                showError($field);
            } else {
                clearError($field);
            }
        });

        // ✅ only run once after validation check
        if (isValid) {
            SaveEventType();
        }
    });

    $(document).off("click", ".editEventType");
    $(document).on("click", ".editEventType", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetEventTypeById(id);
    });

    $(document).off("click", ".deleteEventType");
    $(document).on("click", ".deleteEventType", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");

        //e.preventDefault();
        DeleteEventTypeItem(id);
    });

    // End Event Type

    // Start Statuslegend

    $(document).off("click", ".btnAddNewstatusLegend");
    $(document).on("click", ".btnAddNewstatusLegend", function (e) {
        e.preventDefault();
        AddStatusLegend();
    });

    $(document).off("click", ".cancelstatusLegend");
    $(document).on("click", ".cancelstatusLegend", function (e) {
        e.preventDefault();
        $("#addstatusLegend").empty().html('');
        $('li.active').trigger('click')
    });

    $(document).off("click", ".savestatusLegend");
    $(document).on("click", ".savestatusLegend", function (e) {
        e.preventDefault();
        var isValid = true;

        $("#savestatusLegendDiv").find("input[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            if (value === "") {
                isValid = false;
                showError($field);
            } else {
                clearError($field);
            }
        });

        // ✅ only run once after validation check
        if (isValid) {
            SaveStatusLegend();
        }
    });

    $(document).off("click", ".editStatusLegend");
    $(document).on("click", ".editStatusLegend", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetStatusLegendById(id);
    });

    $(document).off("click", ".deleteStatusLegend");
    $(document).on("click", ".deleteStatusLegend", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        DeleteStatusLegendItem(id);
    });

    // End Statuslegend

    // Start Severity Level
    $(document).off("click", ".btnAddNewseverityLevl");
    $(document).on("click", ".btnAddNewseverityLevl", function (e) {
        e.preventDefault();
        AddSeverity();
    });

    $(document).off("click", ".cancelSeverityLevel");
    $(document).on("click", ".cancelSeverityLevel", function (e) {
        e.preventDefault();
        $("#addseverityLevl").empty().html('');
        $('li.active').trigger('click')
    });

    $(document).off("click", ".saveSeverityLevel");
    $(document).on("click", ".saveSeverityLevel", function (e) {
        e.preventDefault();
        var isValid = true;

        $("#saveSeverityLevelDiv").find("input[data-val-required], textarea[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());

            if (value === "") {
                isValid = false;
                showError($field);
            } else {
                clearError($field);
            }
        });

        // ✅ only run once after validation check
        if (isValid) {
            SaveSeverity();
        }
    });

    $(document).off("click", ".editSeverityLevel");
    $(document).on("click", ".editSeverityLevel", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetSeverityById(id);
    });

    $(document).off("click", ".deleteSeverityLevel");
    $(document).on("click", ".deleteSeverityLevel", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        DeleteSeverityItem(id);
    });
    // End Severity Level

    // === AssetId Handlers ===
    $(document).off("click", ".btnAddNewAsset");
    $(document).on("click", ".btnAddNewAsset", function (e) {
        e.preventDefault();
        AddAssetId();
    });

    $(document).off("click", ".cancelAsset");
    $(document).on("click", ".cancelAsset", function (e) {
        e.preventDefault();
        $("#addAsset").empty().html('');
        $('li.active').trigger('click')
    });


    $(document).on("click", ".saveAsset", function (e) {
        e.preventDefault();
        var isValid = true;
        $("#saveAssetDiv").find("input[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());
            if (value === "") {
                isValid = false;
                showError($field);
            } else {
                clearError($field);
            }
        });

        if (isValid) {
            SaveAssetId();
        }
    });

    $(document).off("click", ".editAsset");
    $(document).on("click", ".editAsset", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetAssetIdById(id);
    });

    $(document).off("click", ".deleteAsset");
    $(document).on("click", ".deleteAsset", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        DeleteAssetItem(id);
    });

    // === IncidentTeam Handlers ===
    $(document).off("click", ".btnAddNewIncidentTeam");
    $(document).on("click", ".btnAddNewIncidentTeam", function (e) {
        e.preventDefault();
        AddIncidentTeam();
    });

    $(document).off("click", ".cancelIncidentTeam");
    $(document).on("click", ".cancelIncidentTeam", function (e) {
        e.preventDefault();
        $("#addIncidentTeam").empty().html('');
        $('li.active').trigger('click');
    });

    $(document).on("click", ".saveIncidentTeam", function (e) {
        e.preventDefault();
        var isValid = true;
        $("#saveIncidentTeamDiv").find("input[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());
            if (value === "") {
                isValid = false;
                showError($field);
            } else {
                clearError($field);
            }
        });

        if (isValid) {
            SaveIncidentTeam();
        }
    });

    $(document).off("click", ".editIncidentTeam");
    $(document).on("click", ".editIncidentTeam", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetIncidentTeamById(id);
    });

    $(document).off("click", ".deleteIncidentTeam");
    $(document).on("click", ".deleteIncidentTeam", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        DeleteIncidentTeamItem(id);
    });


    // === AssetType Handlers ===
    $(document).off("click", ".btnAddNewAssetType");
    $(document).on("click", ".btnAddNewAssetType", function (e) {
        e.preventDefault();
        AddAssetType();
    });

    $(document).off("click", ".cancelAssetType");
    $(document).on("click", ".cancelAssetType", function (e) {
        e.preventDefault();
        $("#addAssetType").empty().html('');
        $('li.active').trigger('click')
    });


    $(document).on("click", ".saveAssetType", function (e) {
        e.preventDefault();
        var isValid = true;
        $("#saveAssetTypeDiv").find("input[data-val-required]").each(function () {
            var $field = $(this);
            var value = $.trim($field.val());
            if (value === "") {
                isValid = false;
                showError($field);
            } else {
                clearError($field);
            }
        });

        if (isValid) {
            SaveAssetType();
        }
    });

    $(document).off("click", ".editAssetType");
    $(document).on("click", ".editAssetType", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        GetAssetTypeById(id);
    });

    $(document).off("click", ".deleteAssetType");
    $(document).on("click", ".deleteAssetType", function (e) {
        e.preventDefault();
        var id = $(this).attr("id");
        DeleteAssetTypeItem(id);
    });

    // === Validation Helpers ===
    function showError($field) { $field.css("border", "1px solid red"); }
    function clearError($field) { $field.css("border", ""); $field.siblings(".field-validation-error").remove(); }



    // Show error: red border + message
    function showError($field) {
        $field.css("border", "1px solid red");
    }

    // Clear error: reset border + remove message
    function clearError($field) {
        $field.css("border", ""); // reset to default
        $field.siblings(".field-validation-error").remove();
    }
})

// Start Source
async function GetAllRelationships() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetAllRelationships", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load source list");

        const content = await response.text();
        $("#sourceList").empty().html(content);

    } catch (error) {
        console.error("Error loading source list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function AddRelationships() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/AddRelationships", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load source list");

        const content = await response.text();
        $("#addSource").empty().html(content);

    } catch (error) {
        console.error("Error loading source list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function GetRelationshipById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetRelationshipById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load source list");

        const content = await response.text();
        $("#addSource").empty().html(content);

    } catch (error) {
        console.error("Error loading source list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function DeleteRelationshipById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/DeleteRelationshipById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load source list");

        SwalSuccessAlert("Source deleted successfully!");
        GetAllRelationships();

    } catch (error) {
        console.error("Error loading source list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function SaveRelationships() {
    try {

        let form = [];
        let formData = new FormData();
        let obj = $("#NewSourceForm")[0];

        // Serialize other fields
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
            form.push({ name: val.name, value: val.value });
        });


        showLoader($(".setting"));

        //console.log(formData);
        console.log(form);

        // Send request
        let response = await fetch("/Settings/SaveRelation", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            $("#addSource").html("");
            SwalSuccessAlert(result.data);
            GetAllRelationships();
        } else {
            SwalErrorAlert(result.message || "Failed to save relation.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving relation!");
        console.error(error);
    } finally {
        hideLoader($(".setting"));
    }
}
function DeleteSourceItem(id) {   // <-- accept id
    let confirmBtnText = "Yes, delete it!";
    let cancelBtnText = "No, cancel!";

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success me-2',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.isConfirmed) {   // ✅ correct way
            DeleteRelationshipById(id);
        }
    });
}
// End Source


// Start Event Type
async function GetAllEventTypes() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetAllEventTypes", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load event type list");

        const content = await response.text();
        $("#eventTypeList").empty().html(content);

    } catch (error) {
        console.error("Error loading event type list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function AddEventType() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/AddEventType", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load event type list");

        const content = await response.text();
        $("#addEventType").empty().html(content);

    } catch (error) {
        console.error("Error loading event type list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function GetEventTypeById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetEventTypeById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load event type list");

        const content = await response.text();
        $("#addEventType").empty().html(content);

    } catch (error) {
        console.error("Error loading event type list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function DeleteEventTypeById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/DeleteEventTypeById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load event type list");

        SwalSuccessAlert("Event Type deleted successfully!");
        GetAllEventTypes();

    } catch (error) {
        console.error("Error loading event type list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function SaveEventType() {
    try {

        let form = [];
        let formData = new FormData();
        let obj = $("#NewEventTypeForm")[0];

        // Serialize other fields
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
            form.push({ name: val.name, value: val.value });
        });


        showLoader($(".setting"));

        //console.log(formData);
        console.log(form);

        // Send request
        let response = await fetch("/Settings/SaveEventType", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            $("#addEventType").html("");
            SwalSuccessAlert(result.data);
            GetAllEventTypes();
        } else {
            SwalErrorAlert(result.message || "Failed to save event type.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving event type!");
        console.error(error);
    } finally {
        hideLoader($(".setting"));
    }
}
function DeleteEventTypeItem(id) {   // <-- accept id
    let confirmBtnText = "Yes, delete it!";
    let cancelBtnText = "No, cancel!";

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success me-2',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.isConfirmed) {   // ✅ correct way
            DeleteEventTypeById(id);
        }
    });
}
// End Event Type



// Start Severity Level
async function GetAllSeverity() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetAllSeverity", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load severity level list");

        const content = await response.text();
        $("#severityLevlList").empty().html(content);

    } catch (error) {
        console.error("Error loading severity level list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function AddSeverity() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/AddSeverity", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to add severity level");

        const content = await response.text();
        $("#addseverityLevl").empty().html(content);

    } catch (error) {
        console.error("Failed to add severity level:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function GetSeverityById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetSeverityById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to get severity level");

        const content = await response.text();
        $("#addseverityLevl").empty().html(content);

    } catch (error) {
        console.error("Failed to get severity level:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function DeleteSeverityById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/DeleteSeverityById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to delete severity level.");

        SwalSuccessAlert("Severity level deleted successfully!");
        GetAllSeverity();

    } catch (error) {
        console.error("Failed to delete severity level:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function SaveSeverity() {
    try {

        let form = [];
        let formData = new FormData();
        let obj = $("#NewSeverityLevelForm")[0];

        // Serialize other fields
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
            form.push({ name: val.name, value: val.value });
        });


        showLoader($(".setting"));

        //console.log(formData);
        console.log(form);

        // Send request
        let response = await fetch("/Settings/SaveSeverity", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            $("#addseverityLevl").html("");
            SwalSuccessAlert(result.data);
            GetAllSeverity();
        } else {
            SwalErrorAlert(result.message || "Failed to save severity level.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving severity level!");
        console.error(error);
    } finally {
        hideLoader($(".setting"));
    }
}
function DeleteSeverityItem(id) {   // <-- accept id
    let confirmBtnText = "Yes, delete it!";
    let cancelBtnText = "No, cancel!";

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success me-2',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.isConfirmed) {   // ✅ correct way
            DeleteSeverityById(id);
        }
    });
}
// End Severity Level


// Start Status legend
async function GetAllStatusLegend() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetAllStatusLegend", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load Status legend list");

        const content = await response.text();
        $("#statusLegendList").empty().html(content);

    } catch (error) {
        console.error("Error loading Status legend list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function AddStatusLegend() {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/AddStatusLegend", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to add Status legend");

        const content = await response.text();
        $("#addstatusLegend").empty().html(content);

    } catch (error) {
        console.error("Failed to add Status legend:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function GetStatusLegendById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/GetStatusLegendById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to get status Legend");

        const content = await response.text();
        $("#addstatusLegend").empty().html(content);

    } catch (error) {
        console.error("Failed to get status Legend:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function DeleteStatusLegendById(id) {
    try {

        showLoader($(".setting"));

        const response = await fetch("/Settings/DeleteStatusLegendById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to delete status legend.");

        SwalSuccessAlert("status legend deleted successfully!");
        GetAllStatusLegend();

    } catch (error) {
        console.error("Failed to delete status legend:", error);
    } finally {
        hideLoader($(".setting"));
    }
}
async function SaveStatusLegend() {
    try {

        let form = [];
        let formData = new FormData();
        let obj = $("#NewstatusLegendForm")[0];

        // Serialize other fields
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
            form.push({ name: val.name, value: val.value });
        });


        showLoader($(".setting"));

        //console.log(formData);
        console.log(form);

        // Send request
        let response = await fetch("/Settings/SaveStatusLegend", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            $("#addstatusLegend").html("");
            SwalSuccessAlert(result.data);
            GetAllStatusLegend();
        } else {
            SwalErrorAlert(result.message || "Failed to save Status Legend.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving Status Legend!");
        console.error(error);
    } finally {
        hideLoader($(".setting"));
    }
}
function DeleteStatusLegendItem(id) {   // <-- accept id
    let confirmBtnText = "Yes, delete it!";
    let cancelBtnText = "No, cancel!";

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: confirmBtnText,
        cancelButtonText: cancelBtnText,
        confirmButtonClass: 'btn btn-success me-2',
        cancelButtonClass: 'btn btn-danger',
        buttonsStyling: false
    }).then(function (result) {
        if (result.isConfirmed) {   // ✅ correct way
            DeleteStatusLegendById(id);
        }
    });
}
// End Status legend


async function GetAllAssetIds() {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/GetAllAssetIds", { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load asset list");
        const content = await response.text();
        $("#assetList").empty().html(content);
    } catch (error) { console.error("Error loading asset list:", error); }
    finally { hideLoader($(".setting")); }
}

async function AddAssetId() {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/AddAssetId", { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load asset form");
        const content = await response.text();
        $("#addAsset").empty().html(content);
    } catch (error) { console.error("Error loading asset form:", error); }
    finally { hideLoader($(".setting")); }
}

async function GetAssetIdById(id) {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/GetAssetIdById?id=" + id, { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load asset");
        const content = await response.text();
        $("#addAsset").empty().html(content);
    } catch (error) { console.error("Error loading asset:", error); }
    finally { hideLoader($(".setting")); }
}

async function DeleteAssetIdById(id) {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/DeleteAssetIdById?id=" + id, { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to delete asset");
        SwalSuccessAlert("Asset deleted successfully!");
        GetAllAssetIds();
    } catch (error) { console.error("Error deleting asset:", error); }
    finally { hideLoader($(".setting")); }
}

async function SaveAssetId() {
    try {
        let form = [], formData = new FormData(), obj = $("#NewAssetForm")[0];
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) { formData.append(val.name, val.value); form.push({ name: val.name, value: val.value }); });
        showLoader($(".setting"));
        let response = await fetch("/Settings/SaveAssetId", { method: "POST", body: formData });
        let result = await response.json();
        if (result.success) {
            $("#addAsset").html("");
            SwalSuccessAlert(result.data);
            GetAllAssetIds();
        } else { SwalErrorAlert(result.message || "Failed to save asset."); }
    } catch (error) { SwalErrorAlert("Error while saving asset!"); console.error(error); }
    finally { hideLoader($(".setting")); }
}

function DeleteAssetItem(id) {
    Swal.fire({
        title: 'Are you sure?', text: "You won't be able to revert this!", icon: 'warning',
        showCancelButton: true, confirmButtonText: "Yes, delete it!", cancelButtonText: "No, cancel!",
        confirmButtonClass: 'btn btn-success me-2', cancelButtonClass: 'btn btn-danger', buttonsStyling: false
    }).then(function (result) { if (result.isConfirmed) { DeleteAssetIdById(id); } });
}


async function GetAllAssetTypes() {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/GetAllAssetTypes", { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load asset types");
        const content = await response.text();
        $("#assetTypeList").empty().html(content);
    } catch (error) { console.error("Error loading asset types:", error); }
    finally { hideLoader($(".setting")); }
}

async function AddAssetType() {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/AddAssetType", { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load asset type form");
        const content = await response.text();
        $("#addAssetType").empty().html(content);
    } catch (error) { console.error("Error loading asset type form:", error); }
    finally { hideLoader($(".setting")); }
}

async function GetAssetTypeById(id) {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/GetAssetTypeById?id=" + id, { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load asset type");
        const content = await response.text();
        $("#addAssetType").empty().html(content);
    } catch (error) { console.error("Error loading asset type:", error); }
    finally { hideLoader($(".setting")); }
}

async function SaveAssetType() {
    try {
        let form = [], formData = new FormData(), obj = $("#NewAssetTypeForm")[0];
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) { formData.append(val.name, val.value); form.push({ name: val.name, value: val.value }); });
        showLoader($(".setting"));
        let response = await fetch("/Settings/SaveAssetType", { method: "POST", body: formData });
        let result = await response.json();
        if (result.success) {
            $("#addAssetType").html("");
            SwalSuccessAlert(result.data || "Asset type saved successfully!");
            GetAllAssetTypes();
        } else { SwalErrorAlert(result.message || "Failed to save asset type."); }
    } catch (error) { SwalErrorAlert("Error while saving asset type!"); console.error(error); }
    finally { hideLoader($(".setting")); }
}

async function DeleteAssetTypeById(id) {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/DeleteAssetTypeById?id=" + id, { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to delete asset type");
        SwalSuccessAlert("Asset type deleted successfully!");
        GetAllAssetTypes();
    } catch (error) { console.error("Error deleting asset type:", error); }
    finally { hideLoader($(".setting")); }
}

function DeleteAssetTypeItem(id) {
    Swal.fire({
        title: 'Are you sure?', text: "You won't be able to revert this!", icon: 'warning',
        showCancelButton: true, confirmButtonText: "Yes, delete it!", cancelButtonText: "No, cancel!",
        confirmButtonClass: 'btn btn-success me-2', cancelButtonClass: 'btn btn-danger', buttonsStyling: false
    }).then(function (result) { if (result.isConfirmed) { DeleteAssetTypeById(id); } });
}

async function GetAllIncidentTeams() {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/GetAllIncidentTeams", { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load incident team list");
        const content = await response.text();
        $("#incidentTeamList").empty().html(content);
    } catch (error) { console.error("Error loading incident team list:", error); }
    finally { hideLoader($(".setting")); }
}

async function AddIncidentTeam() {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/AddIncidentTeam", { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load incident team form");
        const content = await response.text();
        $("#addIncidentTeam").empty().html(content);
    } catch (error) { console.error("Error loading incident team form:", error); }
    finally { hideLoader($(".setting")); }
}

async function GetIncidentTeamById(id) {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/GetIncidentTeamById?id=" + id, { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load incident team");
        const content = await response.text();
        $("#addIncidentTeam").empty().html(content);
    } catch (error) { console.error("Error loading incident team:", error); }
    finally { hideLoader($(".setting")); }
}

async function DeleteIncidentTeamById(id) {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/DeleteIncidentTeamById?id=" + id, { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to delete incident team");
        SwalSuccessAlert("Incident team deleted successfully!");
        GetAllIncidentTeams();
    } catch (error) { console.error("Error deleting incident team:", error); }
    finally { hideLoader($(".setting")); }
}

async function SaveIncidentTeam() {
    try {
        let form = [], formData = new FormData(), obj = $("#NewIncidentTeamForm")[0];
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) { formData.append(val.name, val.value); form.push({ name: val.name, value: val.value }); });
        showLoader($(".setting"));
        let response = await fetch("/Settings/SaveIncidentTeam", { method: "POST", body: formData });
        let result = await response.json();
        if (result.success) {
            $("#addIncidentTeam").html("");
            SwalSuccessAlert(result.data);
            GetAllIncidentTeams();
        } else { SwalErrorAlert(result.message || "Failed to save incident team."); }
    } catch (error) { SwalErrorAlert("Error while saving incident team!"); console.error(error); }
    finally { hideLoader($(".setting")); }
}

function DeleteIncidentTeamItem(id) {
    Swal.fire({
        title: 'Are you sure?', text: "You won't be able to revert this!", icon: 'warning',
        showCancelButton: true, confirmButtonText: "Yes, delete it!", cancelButtonText: "No, cancel!",
        confirmButtonClass: 'btn btn-success me-2', cancelButtonClass: 'btn btn-danger', buttonsStyling: false
    }).then(function (result) { if (result.isConfirmed) { DeleteIncidentTeamById(id); } });
}