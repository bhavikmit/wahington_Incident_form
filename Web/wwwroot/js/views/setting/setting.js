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
        else if (tab === "teamManagement") {
            //GetAllIncidentTeams();
            //$(".teamAllTab.active").trigger("click");
            $(".teamSiderbar ul li:eq(0)").trigger('click');
            $(".teamSiderbar ul li:eq(0)").addClass("active");
        }
    });
    // end setting tabs
   
    $(document).off("click", ".teamAllTab");
    $(document).on("click", ".teamAllTab", function (e) {
        e.preventDefault();
        var tab = $(this).attr("data-tab");

        if (tab === "Ipolicies") {
            // load list when policies tab clicked
            GetAllPolicies();

            // open add policy partial
            $(document).off("click", ".btnAddNewPolicy");
            $(document).on("click", ".btnAddNewPolicy", function (e) {
                e.preventDefault();
                AddPolicy();
            });

            // cancel inside partial
            $(document).off("click", ".cancelPolicy");
            $(document).on("click", ".cancelPolicy", function (e) {
                e.preventDefault();
                $("#addPolicy").empty().html('');
                $('li.active').trigger('click');
            });

            // delete policy
            $(document).off("click", ".deletePolicy");
            $(document).on("click", ".deletePolicy", function (e) {
                e.preventDefault();
                var id = $(this).attr("id");
                DeletePolicyById(id);
            });

            // edit policy
            $(document).off("click", ".editPolicy");
            $(document).on("click", ".editPolicy", function (e) {
                e.preventDefault();
                var id = $(this).attr("id");
                GetPolicyById(id);
            });

            // save policy (client-side required check + submit)
            $(document).off("click", ".savePolicy");
            $(document).on("click", ".savePolicy", function (e) {
                e.preventDefault();
                var isValid = true;

                $("#savePolicyDiv").find("input[data-val-required], textarea[data-val-required]").each(function () {
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
                    SavePolicy();
                }
            });
        } else if (tab === "Iteams") {
            GetAllIncidentTeams();
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
        }
    });


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

// ----------------- setting.js (paste/replace) -----------------

// Utility functions: showLoader/hideLoader/alerts assumed present in your project

// === AJAX loaders ===
async function GetAllIncidentTeams() {
    try {
        showLoader($(".setting"));
        const response = await fetch("/Settings/GetAllIncidentTeams", { method: "GET", headers: { "Content-Type": "application/json", "Accept": "text/html" } });
        if (!response.ok) throw new Error("Failed to load incident team list");
        const content = await response.text();
        $("#incidentTeamList").empty().html(content);
        $("#Iteams").addClass("active");
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
        if (typeof InitIncidentTeamPartial === "function") InitIncidentTeamPartial();
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
        if (typeof InitIncidentTeamPartial === "function") InitIncidentTeamPartial();
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
        // run client-side validation before anything else
        if (!validateIncidentTeamForm()) {
            // validation plugin already shows messages; show a friendly toast as well if you like
            SwalErrorAlert("Please fix validation errors before submitting.");
            return;
        }

        // ensure correct indexing of dynamic list names
        if (typeof window.reindexIncidentSpecializations === "function") {
            window.reindexIncidentSpecializations();
        }

        let formData = new FormData(), obj = $("#NewIncidentTeamForm")[0];
        if (!obj) { SwalErrorAlert("Form not found!"); return; }
        let params = $(obj).serializeArray();

        $.each(params, function (i, val) {
            console.log("adding to formData", val.name, "=", val.value); // Debug log (optional)
            formData.append(val.name, val.value);
        });

        showLoader($(".setting"));
        let response = await fetch("/Settings/SaveIncidentTeam", { method: "POST", body: formData });
        let result = await response.json();

        if (result.success) {
            $("#addIncidentTeam").html("");
            SwalSuccessAlert(result.data);
            GetAllIncidentTeams();
        } else {
            SwalErrorAlert(result.message || "Failed to save incident team.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving incident team!");
        console.error(error);
    } finally {
        hideLoader($(".setting"));
    }
}
function validateIncidentTeamForm() {
    // ensure form exists
    const $form = $("#NewIncidentTeamForm");
    if (!$form || $form.length === 0) {
        console.warn("NewIncidentTeamForm not found for validation.");
        return false;
    }

    // If the plugin hasn't been initialized, initialize with rules.
    // Calling .validate() multiple times is safe; .form() simply validates current state.
    $form.validate({
        rules: {
            // use the actual name attributes of your inputs. Update if different.
            Name: { required: true, minlength: 3 },
            Description: { required: true }
        },
        messages: {
            Name: { required: "Team name is required", minlength: "At least 3 characters" },
            Description: { required: "Description is required" }
        },
        errorClass: "text-danger",
        errorElement: "span",
        // place errors next to bootstrap form-control (adjust selectors if using different markup)
        errorPlacement: function (error, element) {
            // if using bootstrap input-group or custom layout, adjust accordingly
            if (element.parent(".input-group").length) {
                error.insertAfter(element.parent());
            } else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) {
            $(element).addClass("is-invalid");
        },
        unhighlight: function (element) {
            $(element).removeClass("is-invalid");
        }
    });

    // validate and return boolean
    return $form.valid();
}

/*
 * Recommended: call this inside your partial init function so validation rules attach
 * when the partial is loaded via AddIncidentTeam / GetIncidentTeamById.
 *
 * Example partial initializer:
 */
function InitIncidentTeamPartial() {
    // attach validation rules to the partial form
    validateIncidentTeamForm();

    // any other init code: bind events for dynamic specializations, datepickers, etc.
    // e.g. $("#NewIncidentTeamForm").on("submit", function(e){ e.preventDefault(); SaveIncidentTeam(); });
}

function DeleteIncidentTeamItem(id) {
    Swal.fire({
        title: 'Are you sure?', text: "You won't be able to revert this!", icon: 'warning',
        showCancelButton: true, confirmButtonText: "Yes, delete it!", cancelButtonText: "No, cancel!",
        customClass: { confirmButton: 'btn btn-success me-2', cancelButton: 'btn btn-danger' },
        buttonsStyling: false
    }).then(function (result) { if (result.isConfirmed) { DeleteIncidentTeamById(id); } });
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
async function GetAllPolicies() {
    try {
        showLoader($(".setting"));

        const response = await fetch("/Settings/GetAllPolicies", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load policy list");

        const content = await response.text();
        $("#policyList").empty().html(content);

    } catch (error) {
        console.error("Error loading policy list:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function AddPolicy() {
    try {
        showLoader($(".setting"));

        const response = await fetch("/Settings/AddPolicy", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load add policy partial");

        const content = await response.text();
        $("#addPolicy").empty().html(content);

    } catch (error) {
        console.error("Error loading add policy partial:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function GetPolicyById(id) {
    try {
        showLoader($(".setting"));

        const response = await fetch("/Settings/GetPolicyById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to load policy item");

        const content = await response.text();
        $("#addPolicy").empty().html(content);

    } catch (error) {
        console.error("Error loading policy item:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function DeletePolicyById(id) {
    try {
        showLoader($(".setting"));

        const response = await fetch("/Settings/DeletePolicyById?id=" + id, {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
        });

        if (!response.ok) throw new Error("Failed to delete policy");

        SwalSuccessAlert("Policy deleted successfully!");
        GetAllPolicies();

    } catch (error) {
        console.error("Error deleting policy:", error);
    } finally {
        hideLoader($(".setting"));
    }
}

async function SavePolicy() {
    try {
        let form = [];
        let formData = new FormData();
        let obj = $("#NewPolicyForm")[0];

        // Serialize other fields
        let params = $(obj).serializeArray();
        $.each(params, function (i, val) {
            formData.append(val.name, val.value);
            form.push({ name: val.name, value: val.value });
        });

        showLoader($(".setting"));

        console.log(form);

        // Send request
        let response = await fetch("/Settings/SavePolicy", {
            method: "POST",
            body: formData
        });

        let result = await response.json();

        if (result.success) {
            $("#addPolicy").html("");
            SwalSuccessAlert(result.data);
            GetAllPolicies();
        } else {
            SwalErrorAlert(result.message || "Failed to save policy.");
        }
    } catch (error) {
        SwalErrorAlert("Error while saving policy!");
        console.error(error);
    } finally {
        hideLoader($(".setting"));
    }
}

function DeletePolicyItem(id) {
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
        if (result.isConfirmed) {
            DeletePolicyById(id);
        }
    });
}
// End Policy
