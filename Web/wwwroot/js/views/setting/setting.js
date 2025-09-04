$(function () {
    GetAllRelationships();
})

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
