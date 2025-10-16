$(function () {

    $(document).off("change", "#ddlStatus, #ddlOwner");
    $(document).on("change", "#ddlStatus, #ddlOwner", function (e) {

        var statusID = $("#ddlStatus").val() != "" ? $("#ddlStatus").val() : 0;
        var ownerId = $("#ddlOwner").val() != "" ? $("#ddlOwner").val() : 0;
        var step = $("#global_search_value").val() != "" ? $("#global_search_value").val() : "";

        e.preventDefault();
        GetAssessmentDetails(statusID, ownerId, step);
    });

    $(document).off("keyup", "#global_search_value");
    $(document).on("keyup", "#global_search_value", function (e) {
        var step = $(this).val().trim();
        var statusID = $("#ddlStatus").val() != "" ? $("#ddlStatus").val() : 0;
        var ownerId = $("#ddlOwner").val() != "" ? $("#ddlOwner").val() : 0;

        e.preventDefault();

        if (step.length >= 3) {
            GetAssessmentDetails(statusID, ownerId, step);
        }
        else {
            GetAssessmentDetails(statusID, ownerId, "");
        }
    });

    $(document).off("click", "#btnUpdateAssessment");
    $(document).on("click", "#btnUpdateAssessment", async function (e) {

        showLoader($("#updateIncidentAssestmentModal"));

        e.preventDefault();

        const formData = new FormData();

        // Collect basic fields
        formData.append("Id", document.getElementById("assessmentId").value);
        formData.append("StatusId", document.getElementById("status").value);
        formData.append("AssigneeId", document.getElementById("assignee").value);
        formData.append("StartedTime", document.getElementById("startedTime").value);
        formData.append("CompletedTime", document.getElementById("completedTime").value);
        formData.append("Description", document.getElementById("description").value);
        formData.append("MainStepId", document.getElementById("mainstepId").value);
        formData.append("SubStepId", document.getElementById("substepId").value);
        formData.append("IncidentId", document.getElementById("hdnIncidentID").value);


        // Append files (multiple)
        const files = document.getElementById("fileInputAssestment").files;
        for (let i = 0; i < files.length; i++) {
            formData.append("Files", files[i]);
        }

        try {
            const response = await fetch("/IncidentDetail/UpdateAssessment", {
                method: "POST",
                body: formData
            });

            if (response.ok) {
                const result = await response.json();
                if (result.success) {
                    var openTaskCount = result.asssetDetails.OpenTaskCount;
                    var completedTaskCount = result.asssetDetails.CompletedTaskCount;

                    $("#assessment").find("#openTaskCount").text(openTaskCount);
                    $("#assessment").find("#completedTaskCount").text(completedTaskCount);

                    SwalSuccessAlert("Updated Successfully");

                    // Optional: close modal and refresh table
                    $("#updateIncidentAssestmentModal").modal("hide");

                    var statusID = $("#ddlStatus").val() != "" ? $("#ddlStatus").val() : 0;
                    var ownerId = $("#ddlOwner").val() != "" ? $("#ddlOwner").val() : 0;
                    var step = $("#global_search_value").val() != "" ? $("#global_search_value").val() : "";


                    GetAssessmentDetails(statusID, ownerId, step);

                    if (result.partials) {
                        $("#div_Attachments").empty().html(result.partials.viewattachment);
                    }

                    hideLoader($("#updateIncidentAssestmentModal"));



                } else {
                    SwalErrorAlert(result.message || "Update failed.");
                    hideLoader($("#updateIncidentAssestmentModal"));
                }
            } else {
                SwalErrorAlert(result.message || "Update failed.");
                hideLoader($("#updateIncidentAssestmentModal"));
            }
            hideLoader($("#updateIncidentAssestmentModal"));
        } catch (error) {
            console.error("Error:", error);
            SwalErrorAlert(result.message || "Update failed.");
            hideLoader($("#updateIncidentAssestmentModal"));
        }
    });

    $(document).off("change", "#fileInputAssestment");
    $(document).on("change", "#fileInputAssestment", function () {
        const $previewContainer = $('#previewContainerAssestment');
        $previewContainer.empty(); // Clear previous previews

        const files = Array.from(this.files); // Convert FileList to array

        files.forEach(file => {
            const reader = new FileReader();

            reader.onload = function (e) {
                const $preview = $('<div class="preview"></div>').css({
                    width: '100px',
                    height: '100px',
                    overflow: 'hidden',
                    border: '1px solid #ddd',
                    borderRadius: '5px',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    marginRight: '8px'
                }).append(`<img src="${e.target.result}" alt="Image Preview" style="max-width:100%; max-height:100%;">`);

                $previewContainer.append($preview);
            };

            reader.readAsDataURL(file);
        });
    });

});

async function GetAssessmentDetails(statusID, ownerId, step) {
    try {


        let payload = {
            IncidentId: $("#hdnIncidentID").val(),
            step: step,
            statusID: statusID,
            ownerId: ownerId
        };

        showLoader($("#div_assestment_details"));

        const response = await fetch("/IncidentDetail/GetAssessmentDetails", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "text/html"
            },
            body: JSON.stringify(payload)
        });

        if (!response.ok) throw new Error("Failed to load incident list");

        const content = await response.text();
        $("#div_assestment_details").empty().html(content);

    } catch (error) {
        console.error("Error loading incident list:", error);
    } finally {
        hideLoader($("#div_assestment_details"));
    }
}

async function EditAssessmentDetails(id, mainstepId, substepId) {
    try {
        showLoader($("#div_assestment_details"));

        // Send ID as query string
        const response = await fetch(`/IncidentDetail/EditAssessmentDetails?id=${id}&mainstepId=${mainstepId}&substepId=${substepId}`, {
            method: "GET",
            headers: {
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to load incident details");

        const content = await response.text();
        $("#div_assestment_modal").empty().html(content);
        $("#updateIncidentAssestmentModal").modal("show");

    } catch (error) {
        console.error("Error loading incident details:", error);
    } finally {
        hideLoader($("#div_assestment_details"));
    }
}

async function ViewAssessmentDetails(id, mainstepId, substepId) {
    try {
        showLoader($("#div_assestment_view_modal"));

        // Send ID as query string
        const response = await fetch(`/IncidentDetail/ViewAssessmentDetails?id=${id}&mainstepId=${mainstepId}&substepId=${substepId}`, {
            method: "GET",
            headers: {
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to load incident details");

        const content = await response.text();
        $("#div_assestment_view_modal").empty().html(content);
        $("#viewIncidentAssestmentModal").modal("show");

    } catch (error) {
        console.error("Error loading incident details:", error);
    } finally {
        hideLoader($("#div_assestment_details"));
    }
}
async function OpenIncidentMap(id) {
    try {
        let payload = { id: id };

        showLoader($(".main-content"));

        const url = `/Incidents/GetIncidentMapDetailsbyId?id=${id}`;

        const response = await fetch(url, {
            method: "GET",
            headers: {
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to load incident map");

        const content = await response.text();
        $("#incidentMapContainer").empty().html(content); // 👈 replace with your target div
        $("#MapIncidentModal").modal("show");

    } catch (error) {
        console.error("Error loading incident map:", error);
    } finally {
        hideLoader($(".main-content"));
    }
}

async function AddAssessmentDetails() {
    try {
        showLoader($("#div_assestment_details"));

        // Send ID as query string
        const response = await fetch(`/IncidentDetail/AddAssessmentDetails`, {
            method: "GET",
            headers: {
                "Accept": "text/html"
            }
        });

        if (!response.ok) throw new Error("Failed to load incident details");

        const content = await response.text();
        $("#div_Add_assestment_modal").empty().html(content);
        $("#addIncidentAssestmentModal").modal("show");

    } catch (error) {
        console.error("Error loading incident details:", error);
    } finally {
        hideLoader($("#div_assestment_details"));
    }
}

async function SubmitAssestment() {
    try {
        showLoader($("#div_assestment_details"));

        const formData = new FormData();
        const Assessment = {};

        const mappings = {
            IC_MCR: [".IncidentCommander", "div_CreateMCR"],
            IC_Notify: [".IncidentCommander", "div_NotifyclaimAndEngineering"],
            IC_EstablishICP: [".IncidentCommander", "div_EstablishICP"],
            FER_PCA: [".FieldEnvironmentalRepresentative", "div_Preparecontainmentarea"],
            FER_LC: [".FieldEnvironmentalRepresentative", "div_Labelcontainers"],
            EGEC_RSM: [".EngineeringAndGEC", "div_Retrievesystemmaps"],
            EGEC_MLP: [".EngineeringAndGEC", "div_Marklowpoints"],
            EGEC_ICT: [".EngineeringAndGEC", "div_Initiatecosttracking"]
        };

        $.each(mappings, function (key, [role, div]) {
            const { assignId, statusId } = getAssignAndStatus(role, div) || {};
            Assessment[`${key}_AssignId`] = assignId ?? 0;
            Assessment[`${key}_StatusId`] = statusId ?? 0;
        });

        formData.append("incidentValidationAssessment", JSON.stringify(Assessment));
        formData.append("IncidentId", $("#hdnIncidentID").val() || 0);

        const response = await fetch("/IncidentDetail/SubmitAssestment", {
            method: "POST",
            body: formData
        });

        const result = await response.json();

        if (result.success) {
            SwalSuccessAlert(result.data);

            const statusID = $("#ddlStatus").val() || 0;
            const ownerId = $("#ddlOwner").val() || 0;
            const step = $("#global_search_value").val() || "";

            GetAssessmentDetails(statusID, ownerId, step);
        } else {
            SwalErrorAlert(result.message || "Failed to save Incident Validation.");
        }
    } catch (error) {
        console.error("Error submitting assessment:", error);
        SwalErrorAlert("An unexpected error occurred while submitting assessment.");
    } finally {
        hideLoader($("#div_assestment_details"));
    }
}
