const BACKEND_BASE_URL = "/api";

export async function fetchBookById(id: string) {
  try {
    const response = await fetch(`${BACKEND_BASE_URL}/book/${id}`);

    if (!response.ok) throw new Error("Failed to fetch book");

    const result = await response.json();
    return result.data;
  } catch (err) {
    console.error("Error fetching book:", err);
    return null;
  }
}

export async function requestBorrow(userId: string, bookId: string) {
  try {
    const response = await fetch(`${BACKEND_BASE_URL}/borrow/request`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ userId, bookId }),
    });

    return await response.json();
  } catch (err) {
    console.error("Borrow request failed:", err);
    return { success: false, message: "Failed to send request" };
  }
}
