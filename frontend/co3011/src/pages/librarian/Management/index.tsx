import React from "react";

interface BookDetailProps {
  title: string;
  author: string;
  year: number;
  publisher: string;
  genre: string;
  description: string;
  copies: number;
  rarity: string;
  imageUrl: string;
  onAccept?: () => void;
  onReject?: () => void;
}

const rarityColors: Record<string, string> = {
  General: "text-green-500",
  Popular: "text-green-400",
  Specialized: "text-cyan-400",
  Limited: "text-purple-400",
  Unique: "text-red-500",
};

export default function LBookDetailx({
  title,
  author,
  year,
  publisher,
  genre,
  description,
  copies,
  rarity,
  imageUrl,
  onAccept,
  onReject,
}: BookDetailProps){

return (
  <iframe 
    src="doc.html" 
    width="100%" 
    height="100%" 
    title="Library Dashboard"
></iframe>
)
}

export function LBookDetaily({
  title,
  author,
  year,
  publisher,
  genre,
  description,
  copies,
  rarity,
  imageUrl,
  onAccept,
  onReject,
}: BookDetailProps) {
  return (
    <div className="flex flex-col items-center w-full min-h-screen bg-gray-50 p-6">
      <h2 className="text-xl font-semibold mb-4 border-b border-gray-300 pb-2 w-full text-left">
        Book Detail
      </h2>

      <div className="flex flex-row gap-6 w-full max-w-5xl bg-white shadow-md rounded-2xl p-6">
        {/* Left: Book Image + Legend */}
        <div className="flex flex-col items-center w-1/3">
          <img
            src={imageUrl}
            alt={title}
            className="w-60 h-auto rounded-xl shadow"
          />

          <div className="mt-4 text-sm">
            <p className="text-gray-600">
              <span className="text-green-500">■</span> General Books +200
            </p>
            <p className="text-gray-600">
              <span className="text-green-400">■</span> Popular Books +500
            </p>
            <p className="text-gray-600">
              <span className="text-cyan-400">■</span> Specialized Books +1200
            </p>
            <p className="text-gray-600">
              <span className="text-purple-400">■</span> Limited Copies +2100
            </p>
            <p className="text-gray-600">
              <span className="text-red-500">■</span> Unique Books +3600
            </p>
          </div>
        </div>

        {/* Right: Info Columns */}
        <div className="flex-1 grid grid-cols-2 gap-4">
          {/* Column 1 */}
          <div className="flex flex-col gap-3">
            <InfoBox label="Author" value={author} />
            <InfoBox label="Published Year" value={year.toString()} />
            <InfoBox label="Publisher" value={publisher} />
            <InfoBox label="Available Copy" value={copies.toString()} />

            <div className="bg-gray-100 rounded-xl p-3 text-center">
              <p className="font-semibold text-gray-700 mb-1">Rarity</p>
              <select
                className="border rounded-md p-1 w-32 text-center"
                value={rarity}
                onChange={() => {}}
              >
                {Object.keys(rarityColors).map((r) => (
                  <option key={r} value={r}>
                    {r}
                  </option>
                ))}
              </select>
            </div>
          </div>

          {/* Column 2 */}
          <div className="flex flex-col gap-3">
            <div className="bg-gray-100 rounded-xl p-3 text-center">
              <p className="font-semibold text-gray-700 mb-1">Genre</p>
              <p className="text-lg font-bold">{genre}</p>
            </div>

            <div className="bg-gray-100 rounded-xl p-3">
              <p className="font-semibold text-gray-700 mb-1">Description</p>
              <p className="text-gray-600 text-sm">{description}</p>
            </div>
          </div>
        </div>
      </div>

      {/* Footer Buttons */}
      <div className="flex gap-6 mt-6">
        <button
          className="px-6 py-2 bg-blue-600 hover:bg-blue-700 text-white font-semibold rounded-lg transition"
          onClick={onAccept}
        >
          Accept
        </button>
        <button
          className="px-6 py-2 bg-red-500 hover:bg-red-600 text-white font-semibold rounded-lg transition"
          onClick={onReject}
        >
          Reject
        </button>
      </div>
    </div>
  );
}

function InfoBox({ label, value }: { label: string; value: string }) {
  return (
    <div className="bg-gray-100 rounded-xl p-3 text-center">
      <p className="font-semibold text-gray-700 mb-1">{label}:</p>
      <p className="text-gray-800">{value}</p>
    </div>
  );
}
