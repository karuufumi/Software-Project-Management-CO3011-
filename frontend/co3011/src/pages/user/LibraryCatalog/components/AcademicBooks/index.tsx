import { CustomSlider } from "../../../../../components/slider";
import Widget from "../../../../../components/widget/widget";
import viteLogo from "/vite.svg";

const mockData = [
  {
    title: "Principle of Programming Language",
    image: viteLogo,
  },
  {
    title: "Computer Architecture",
    image: viteLogo,
  },
  {
    title: "Probability & Statistic",
    image: viteLogo,
  },
  {
    title: "Data Structure & Algorithm",
    image: viteLogo,
  },
  {
    title: "Operating System",
    image: viteLogo,
  },
];

const responsive = {
  superLargeDesktop: {
    // the naming can be any, depends on you.
    breakpoint: { max: 4000, min: 3000 },
    items: 5,
  },
  desktop: {
    breakpoint: { max: 3000, min: 1024 },
    items: 3,
  },
  tablet: {
    breakpoint: { max: 1024, min: 464 },
    items: 2,
  },
  mobile: {
    breakpoint: { max: 464, min: 0 },
    items: 1,
  },
};

export function AcademicBooks() {
  return (
    <>
      <h3
        style={{
          fontWeight: 600,
          textTransform: "capitalize",
          marginBottom: 10,
        }}
      >
        academic books
      </h3>

      <CustomSlider
        carouselProps={{
          responsive: responsive,
          children: mockData.map((item) => (
            <Widget marginLeft={30}>
              <p>{item.title}</p>
              <img src={item.image} className="logo" alt="Vite logo" />
            </Widget>
          )),
          infinite: true,
        }}
        style={{
          paddingBlock: 10,
        }}
      />
    </>
  );
}
